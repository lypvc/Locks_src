using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace MyApplic
{
    public class GramerHelper
    {
/*
private const string AsmPath = @"D:\Dev\AsmLoading\AsmLoadingImpl\bin\Debug\AsmLoadingImpl.dll";

    public ILoadable GetExternalLoadable()
    {
        var asmBytes = File.ReadAllBytes(AsmPath);
        Assembly asm = Assembly.Load(asmBytes, null);
        var loadableInterface = typeof(ILoadable);
        var loadableImpl = asm.GetTypes().First(loadableInterface.IsAssignableFrom);
        return (ILoadable)Activator.CreateInstance(loadableImpl);
    }
*/
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void BuilderNewApp()
        {


AppDomain ad = AppDomain.CurrentDomain;

        AssemblyName an = new AssemblyName();
        an.Name = "DynamicRandomAssembly";
        AssemblyBuilder ab = ad.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

        ModuleBuilder mb = ab.DefineDynamicModule("RandomModule");

        TypeBuilder tb = mb.DefineType("DynamicRandomClass",TypeAttributes.Public);

        Type returntype = typeof(int);
        Type[] paramstype = new Type[0];
        MethodBuilder methb=tb.DefineMethod("DynamicRandomMethod", MethodAttributes.Public, returntype, paramstype);

        ILGenerator gen = methb.GetILGenerator();
        gen.Emit(OpCodes.Ldc_I4, 1);
        gen.Emit(OpCodes.Ret);

        Type t = tb.CreateType();

        Object o = Activator.CreateInstance(t);
        Object[] aa = new Object[0];
        MethodInfo m = t.GetMethod("DynamicRandomMethod");
        int i = (int) m.Invoke(o, aa);
        Console.WriteLine("Method {0} in Class {1} returned {2}",m, t, i);


            
            AssemblyName asm = new AssemblyName();
        //获取当前应用程序域:
            AppDomain appdomain = System.Threading.Thread.GetDomain();
            //appdomain.Load()
               var asm= Assembly.GetExecutingAssembly();
               
            var filename = Guid.NewGuid().ToString().Replace("-",".");
            //定义程序集
            AssemblyBuilder abuilder = appdomain.DefineDynamicAssembly(asm, AssemblyBuilderAccess.RunAndSave);
            
            //定义模块
            ModuleBuilder mbuilder = abuilder.DefineDynamicModule(filename,filename+".dll");
            //定义类
            TypeBuilder theClass = mbuilder.DefineType("DynamicInvokeTest.Test", TypeAttributes.Public | TypeAttributes.Class);
            //定义方法或者字段
            //如果方法有参数或者返回值的，应先定义参数或者返回值类型，根据上面的例子，需要传入string 类型的参数，所以定义如--下:
            Type[] param = new Type[1];//定义方法的参数
            param[0] = typeof(System.String);
            MethodBuilder methodBuilder = theClass.DefineMethod("Output", MethodAttributes.Public, null, param);
            //然后根据上面IL代码，定义方法体
            ILGenerator gen = methodBuilder.GetILGenerator();
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Call,typeof(Console).GetMethod("WriteLine",new Type[]{typeof(string)}));
            gen.Emit(OpCodes.Ret);
/*
            //test
            //最后调用我们所创建的程序集
            //创建类型
            Type TestClass = theClass.CreateType();
            //创建对象
            object TestInst = Activator.CreateInstance(TestClass );
            //调用方法
            object[] args = new object[] { "Dynamic Test" };
            TestClass .InvokeMember("Output", BindingFlags.InvokeMethod, null, TestInst , args);
             */
        }

    }
}