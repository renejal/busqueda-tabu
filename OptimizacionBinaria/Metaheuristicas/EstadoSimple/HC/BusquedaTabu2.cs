using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizacionBinaria.Funciones;
using System.Collections;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class BusquedaTabu2 : Algoritmo
    {
        //atributos
        public int MaxLongituLitaTabu;
        public int atrnumeroTweask;
        private Queue atrListaTabu = new Queue();
        public double pm = 0.5;
        public double radio = 10;



        public override void Ejecutar(Knapsack parProblema, Random ParAleatorio)
        {
            EFOs = 0;

            var s = new Solucion(parProblema, this);
            s.InicializarAleatorio(ParAleatorio);
            //
            MejorSolucion = new Solucion(s);
            //agregoa a la sita tabu
            atrListaTabu.Enqueue(MejorSolucion);
            while (EFOs < MaxEFOs)
            {
                if (atrListaTabu.Count >= MaxLongituLitaTabu)
                {
                    atrListaTabu.Dequeue();
                }
                var r = new Solucion(s);
                r.Tweak(ParAleatorio, pm, radio);
                for (int i = 0; i < atrnumeroTweask - 1; i++)
                {
                    var w = new Solucion(s);
                    w.Tweak(ParAleatorio, pm, radio);
                    if (!perteneceListaTabu(w) && (w.fitness > r.fitness || perteneceListaTabu(r)))
                        r = w;
                    if (!perteneceListaTabu(r) && r.fitness > s.fitness)
                    {
                        s = r;
                        atrListaTabu.Enqueue(r);
                    }
                    if (s.fitness > MejorSolucion.fitness)
                        MejorSolucion = s;

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
}
