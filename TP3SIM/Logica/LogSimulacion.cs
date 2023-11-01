using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP3SIM.Entidades;

namespace TP3SIM.Logica
{
    public class LogSimulacion
    {

        private readonly Random RND = new Random();

        public double VariableAleatoriaConvolucion(double media, double desviacion)
        {
            double sumaConvolucion = 0;
            double variableAleatoria;
            double random;

            for (int x = 0; x < 12; x++)
            {
                random = GenerarRND();
                sumaConvolucion += random;
            }

            variableAleatoria = ((sumaConvolucion - 6) * desviacion) + media;

            if (variableAleatoria > 0)
            {
                return Math.Round(variableAleatoria, 2);
            }

            return VariableAleatoriaConvolucion(media, desviacion);
        }

        public double VariableAleatoriaUniforme(double minimo, double maximo, double random)
        {
            double variableAleatoria = minimo + (random * (maximo - minimo));

            return Math.Round(variableAleatoria, 2);
        }

        public double VariableAleatoriaExponencial(double media, double random)
        {
            double variableAleatoria;

            variableAleatoria = -media * Math.Log(1 - random);

            return Math.Round(variableAleatoria, 2);
        }

        public string VariableAleatoriaTipo(double random)
        {
            if(random <= 0 && random < 0.45)
            {
                return "Pedir libro";
            }
            else if (random >= 0.45 && random < 0.9)
            {
                return "Devolver libro";
            }
            return "Consulta";
        }

        public double GenerarRND()
        {
            double random;

            do
            {
                random = Math.Round(RND.NextDouble(), 4);
            } while (random == 0 || random == 1);

            return random;
        }

        public int CantidadPersonasBiblioteca(List<Temporal> personas)
        {
            var cant = 0;
            var destruido = new Destruido();
            foreach (var persona in personas) {
                if(persona.Estado != destruido)
                {
                    cant++;
                }
            }
            return cant;

        }
    }
}
