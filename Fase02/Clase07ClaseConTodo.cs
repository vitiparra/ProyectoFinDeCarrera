using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace Fase02
{
    [ProtoContract]
    [Serializable]
    public class ClaseBase2
    {
        [ProtoMember(1)]
        public int basePublicInt { get; set; }
        [ProtoMember(2)]
        protected string baseProtectedString { get; set; }
    }

    [ProtoContract]
    [Serializable]
    public class Clase07ClaseConTodo : ClaseBase2
    {
        public enum colores
        {
            ROJO,
            AMARILLO,
            VERDE
        }
        [ProtoMember(1)]
        public colores publicStaticColores;

        [ProtoMember(2)]
        [NonSerialized]
        public List<int> lista;

        [ProtoMember(3)]
        public int publicInt { get; set; }

        [ProtoMember(4)]
        public int[] publicArrayInt { get; set; }

        [ProtoMember(5)]
        [NonSerialized]
        public int[,] publicArray2DInt;

        [ProtoMember(6)]
        [NonSerialized]
        public int[][] publicArrayMatrizEscalonadaInt;

        [ProtoMember(7)]
        protected string protectedString { get; set; } // Falta esto

        [ProtoMember(8)]
        private static int privateStaticInt;

        [ProtoMember(9)]
        private float privateFloat { get; set; } // Falta esto
//        public static colores privateStaticColores;

        public Clase07ClaseConTodo()
        {
            this.publicStaticColores = colores.AMARILLO;

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

            Clase07ClaseConTodo.privateStaticInt = 1000;
        }
    }
}
