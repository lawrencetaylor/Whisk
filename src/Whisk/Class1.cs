using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Whisk
{
    public static class MixinHelper
    {
        private static readonly ModuleBuilder ModuleBuilder;
        private static AssemblyBuilder ab;
        private static string name = "MixinAssembly";

        static MixinHelper()
        {
            var an = new AssemblyName(name);
            ab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder = ab.DefineDynamicModule(name, name + ".dll");
        }

        public static Type Create(Type[] typesToMixin, Type[] virtualContracts = null)
        {
            TypeBuilder tb = ModuleBuilder.DefineType(Guid.NewGuid().ToString(),
                TypeAttributes.Public | TypeAttributes.Class);

            var typeFields = typesToMixin.ToDictionary(tf => tf,
                tf => tb.DefineField("_" + tf.Name, tf, FieldAttributes.Private));

            #region Constructor

            var constructorBuilder = tb.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                typesToMixin);

            var il = constructorBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            for (var i = 1; i <= typesToMixin.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg, i);
                il.Emit(OpCodes.Stfld, typeFields[typesToMixin[i - 1]]);
            }
            il.Emit(OpCodes.Ret);

            #endregion

            foreach (var type in typesToMixin.Concat(virtualContracts ?? new Type[0]))
            {
                tb.AddInterfaceImplementation(type);
            }



            foreach (var type in typesToMixin)
            {
                foreach (var method in type.GetMethods())
                {
                    var methodBuilder = tb.DefineMethod(method.Name,
                        MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.Final | MethodAttributes.NewSlot,
                        method.ReturnType,
                        method.GetParameters().Select(p => p.ParameterType).ToArray());
                    il = methodBuilder.GetILGenerator();

                    if (method.ReturnType == typeof(void))
                    {
                        il.Emit(OpCodes.Nop);
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, typeFields[type]);
                        il.Emit(OpCodes.Callvirt, method);
                        il.Emit(OpCodes.Ret);
                    }
                    else
                    {
                        il.DeclareLocal(method.ReturnType);

                        il.Emit(OpCodes.Nop);
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, typeFields[type]);

                        var methodParameterInfos = method.GetParameters();
                        for (int i = 0; i < methodParameterInfos.Length; i++)
                            il.Emit(OpCodes.Ldarg, (i + 1));
                        il.Emit(OpCodes.Callvirt, method);

                        il.Emit(OpCodes.Stloc_0);
                        var defineLabel = il.DefineLabel();
                        il.Emit(OpCodes.Br_S, defineLabel);
                        il.MarkLabel(defineLabel);
                        il.Emit(OpCodes.Ldloc_0);
                        il.Emit(OpCodes.Ret);
                    }

                    tb.DefineMethodOverride(methodBuilder, method);
                }
            }

            var t = tb.CreateType();

            //ab.Save(name + ".dll");

            return t;
        }

        public static Type Create<TMixinFirst, TMixinSecond>()
        {
            return Create(new[] { typeof(TMixinFirst), typeof(TMixinSecond) });
        }

        public static TMixinFirst CreateInstance<TMixinFirst, TMixinSecond>(TMixinFirst firstMixin,
            TMixinSecond secondMixin)
        {
            var firstMixinInterface = typeof(TMixinFirst).GetInterfaces().Single();
            var secondMixinInterface = typeof(TMixinSecond).GetInterfaces().Single();

            var mixinType = Create(new[] { firstMixinInterface, secondMixinInterface });

            return (TMixinFirst)Activator.CreateInstance(mixinType, firstMixin, secondMixin);

        }
    }

    public class MixinBuilder<TMainContract>
    {
        private readonly Dictionary<Type, object> _mixins;

        public MixinBuilder(TMainContract initialObject)
        {
            _mixins = new Dictionary<Type, object> { { typeof(TMainContract), initialObject } };
        }

        public MixinBuilder<TMainContract> AddMixin<TMixin>(TMixin initalObject)
        {
            _mixins.Add(typeof(TMixin), initalObject);
            return this;
        }

        public TMainContract Create()
        {
            var mixinType = MixinHelper.Create(_mixins.Keys.ToArray());
            return (TMainContract)Activator.CreateInstance(mixinType, _mixins.Values.ToArray());
        }

        public TMixin Create<TMixin>()
        {
            var mixinType = MixinHelper.Create(_mixins.Keys.ToArray(), new[] { typeof(TMixin) });
            return (TMixin)Activator.CreateInstance(mixinType, _mixins.Values.ToArray());
        }
    }
}
