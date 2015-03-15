using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public class ClaseBase2
    {
        public int basePublicInt { get; set; }
        protected string baseProtectedString { get; set; }
    }

    public class Clase07ClaseConTodo: ClaseBase2
    {
        public enum colores
        {
            ROJO,
            AMARILLO,
            VERDE
        }
        public static colores publicStaticColores;
        public List<int> lista { get; set; }
        public int publicInt { get; set; }
        public int[] publicArrayInt { get; set; }
        public int[,] publicArray2DInt { get; set; }
        public int[][] publicArrayMatrizEscalonadaInt { get; set; }

        protected string protectedString { get; set; } // Falta esto

        private static int privateStaticInt;
        private float privateFloat { get; set; } // Falta esto
//        public static colores privateStaticColores;

        public Clase07ClaseConTodo()
        {
            this.basePublicInt = 1;
            this.baseProtectedString = "hola";
            this.publicInt = 2;
            this.protectedString = "adios";
            this.privateFloat = 1;

            this.publicArrayInt = new int[4];
            this.publicArrayInt[0] = 1;
            this.publicArrayInt[1] = 2;
            this.publicArrayInt[2] = 3;
            this.publicArrayInt[3] = 4;
            
            this.publicArray2DInt = new int[2,3];
            this.publicArray2DInt[0, 0] = 1;
            this.publicArray2DInt[0, 1] = 2;
            this.publicArray2DInt[0, 2] = 3;
            this.publicArray2DInt[1, 0] = 4;
            this.publicArray2DInt[1, 1] = 5;
            this.publicArray2DInt[1, 2] = 6;

            this.publicArrayMatrizEscalonadaInt = new int[2][];
            this.publicArrayMatrizEscalonadaInt[0] = new int[3];
            this.publicArrayMatrizEscalonadaInt[0][0] = 1;
            this.publicArrayMatrizEscalonadaInt[0][1] = 2;
            this.publicArrayMatrizEscalonadaInt[0][2] = 3;

            this.publicArrayMatrizEscalonadaInt[1] = new int[5];
            this.publicArrayMatrizEscalonadaInt[1][0] = 1;
            this.publicArrayMatrizEscalonadaInt[1][1] = 2;
            this.publicArrayMatrizEscalonadaInt[1][2] = 3;
            this.publicArrayMatrizEscalonadaInt[1][3] = 4;
            this.publicArrayMatrizEscalonadaInt[1][4] = 5;


            this.lista = new List<int>();
            this.lista.Add(1);
            this.lista.Add(2);
        }
    }
}
