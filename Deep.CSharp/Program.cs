using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Deep.CSharp {
    class Program {
        static void Main(string[] args)
        {

            var conClass = new InhClass();
            var absClass = conClass as SuperClass ;

            conClass.HelloInherited();

            Console.WriteLine("-----------");

            absClass.HelloInherited();

            Console.ReadLine();

        }

        
    }

    public abstract class BaseClass {
        public abstract void HelloWorld();
        public int Var1 { get; set; }

        protected BaseClass()
        {
            Var1 = 1;
        }

        public void HelloInherited()
        {
            Console.WriteLine("Class name : [{0}] invoked hello inherited and var1 equal [{1}] ", MethodBase.GetCurrentMethod().DeclaringType, Var1);
        }
    }

    public class ConcreteClass : BaseClass
    {
        public int ConcreteVar { get; set; }
        public ConcreteClass()
        {
            //Var1 = 2;
        }
        public override void HelloWorld() {
            Console.WriteLine("Class name : [{0}] invoked hello inherited and var1 equal [{1}] ", MethodBase.GetCurrentMethod().DeclaringType, Var1);
        }
    }

    public class SuperClass
    {
        public int Var1 { get; set; }

        public SuperClass()
        {
            Var1 = 1;
        }

        public virtual void HelloInherited() {
            Console.WriteLine("Class name : [{0}] invoked hello inherited and var1 equal [{1}] ",MethodBase.GetCurrentMethod().DeclaringType,Var1);
        }
    }

    public class InhClass:SuperClass
    {
        public int Var2 { get; set; }

        public InhClass()
        {
            Var1 = 2;
        }

        public override void HelloInherited()
        {
            Console.WriteLine("Class name : [{0}] invoked hello inherited and var1 equal [{1}] ",MethodBase.GetCurrentMethod().DeclaringType,Var1);
        }
    }




}
