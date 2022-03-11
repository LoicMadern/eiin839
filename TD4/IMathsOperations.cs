using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MathsLibrary
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class IMathsOperations : MathsOperations
    {
        float Add(float op1, float op2)
        {
            return op1 + op2;
        }

        float Substract(float op1, float op2)
        {
            return op1 - op2;
        }

        float Multiplicaty(float op1, float op2)
        {
            return op1 * op2;
        }

       


        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
