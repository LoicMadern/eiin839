﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MathsLibrary
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class MathsOperations : IMathsOperations
    {
  

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

        int IMathsOperations.Add(int a, int b)
        {
            return a + b;
        }

        float IMathsOperations.Divide(int a, int b)
        {
            return a / b;
        }

        int IMathsOperations.Substract(int a, int b)
        {
            return a - b;
        }

        float IMathsOperations.Multiply(int a, int b)
        {
            return a * b;
        }
    }
}
