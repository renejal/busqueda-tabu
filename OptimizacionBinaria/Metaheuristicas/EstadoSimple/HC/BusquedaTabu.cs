using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizacionBinaria.Funciones;
using System.Collections;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class BusquedaTabu : Algoritmo
    {
        //atributos
        public int atrnumeroTweask;
        private ArrayList atrListaTabu = new ArrayList();
        public double pm = 0.5;
        public double radio = 10;
        public int atrIteracionActual;
        public int atrTimeTabu;
       




        public override void Ejecutar(Knapsack parProblema, Random ParAleatorio)
        {
            EFOs = 0;

            var s = new Solucion(parProblema, this);
            s.InicializarAleatorio(ParAleatorio);
            //
            MejorSolucion = s;
            //agregoa a la sita tabu
            this.AddListaCarateristicas(s,1);
            atrIteracionActual = 1;
            while (EFOs < MaxEFOs)
            {
                atrIteracionActual++;
                //remoder de la lista tabu todas las tublas en la iteracion c -d >l
                this.DeleteListaCaracteristicas();
                var r = new Solucion(s);
                r.Tweak(ParAleatorio, pm, radio, atrListaTabu);
                for (int i = 0; i < atrnumeroTweask - 1; i++)
                {
                    var w = new Solucion(s);
                    w.Tweak(ParAleatorio, pm, radio,atrListaTabu);
                    if (w.fitness > r.fitness)
                        r = w;
                    s = r;
                    this.AddListaCarateristicas(s, atrIteracionActual);
                    if (s.fitness > MejorSolucion.fitness)
                        MejorSolucion = s;

                }
            }

        }
        private void AddListaCarateristicas(Solucion parSolucion, int parIteracion)
        {
            //1. objtener el vector solucion
            int[] Dimensione = parSolucion.getDimensiones();

            //2. obtenr las caractirsitcas imersas o unos dentro de la solucion sus dimensiones
            for (int i = 0; i <= Dimensione.Length - 1; i++)
            {
                if (Dimensione[i] == 1)
                {
                  caracteristica objCaracteristica = new caracteristica(i, parIteracion);
                    atrListaTabu.Add(objCaracteristica);
                }
            }
            //3. guradasra en la lista tabu
        }
        private void DeleteListaCaracteristicas()
        {
            for(int i =0; i<=atrListaTabu.Count-1;i++)
            {
                if (atrIteracionActual - ((caracteristica)atrListaTabu[i]).atrIteracion > atrTimeTabu)
                {
                    atrListaTabu.RemoveAt(i);
                }
            }
        }
        private Boolean perteneceListaTabu(Solucion parSolucion)
        {
            Boolean varRespuesta = false;
            foreach (Solucion varSolucion in atrListaTabu)
            {
                if (varSolucion.Equals(parSolucion))
                {
                    varRespuesta = true;
                }
            }
            return varRespuesta;
        }
    }

    public class caracteristica
    {
        public int atrCaracteristica; //valor de la dimension dentro del vector
        public int atrIteracion;

        public caracteristica(int parCarateristica, int parIteracion)
        {
            atrCaracteristica = parCarateristica;
            atrIteracion = parIteracion;
        }

        

    }



}
