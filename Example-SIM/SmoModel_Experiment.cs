using System;
using CommonModel.Kernel;
using CommonModel.RandomStreamProducing;
using System.Collections.Generic;

namespace Model_Lab
{

    public partial class SmoModel : Model
    {
        //Условие завершения прогона модели True - завершить прогон. По умолчанию false. </summary>
        public override bool MustStopRun(int variantCount, int runCount)
        {
            return (Time >= TP);
        }

        //установка метода перебора вариантов модели
        public override bool MustPerformNextVariant(int variantCount)
        {
            //используем один вариант модели
            return variantCount < 1;
        }

        //true - продолжить выполнение ПРОГОНОВ модели;
        //false - прекратить выполнение ПРОГОНОВ модели. по умолчению false.
        public override bool MustPerformNextRun(int variantCount, int runCount)
        {
            return runCount < 1; //выполняем 1 прогон модели
        }

        //Задание начального состояния модели для нового варианта модели
        public override void SetNextVariant(int variantCount)
        {
            #region Параметры модели
            
			MZ = new int[KZ, KUVS + 1] 
			{
				{1,2,3,-1},
				{1,2,1,-1}
			};
            
			MOKK = new int[KZ, KUVS + 1]
            {
                {4,5,4,-1},
                {3,4,5,-1}
            };
            
			MAXKPP = new int[KUVS] {1, 1, 1};

			TP = 100;

            #endregion

            #region Установка параметров законов распределения

			(GenKKZ.BPN as GeneratedBaseRandomStream).Seed = 1;
			(GenTime.BPN as GeneratedBaseRandomStream).Seed = 2;
           


            #endregion
        }

        public override void StartModelling(int variantCount, int runCount)
        {
            #region Задание начальных значений модельных переменных и объектов
            #endregion

            #region Cброс сборщиков статистики

            #endregion

            //Печать заголовка строки состояния модели
            TraceModelHeader();

			#region Планирование начальных событий
            var ev1 = new K1();                                 // создание объекта события
			Bid Z1 = new Bid();
            Z1.NZ = 1;
			Z1.NE = 1;
			Z1.KK = GenKKZ.GenerateValue();
			ev1.ZP = Z1;     
			var rec = new QRec();
            rec.Z = Z1;
            KPP[MZ[0,0]].Add(rec);
            PlanEvent(ev1, 0.0);                          // планирование события 3
			Tracer.PlanEventTrace(ev1);
            
			var ev2 = new K1();                                 // создание объекта события
            Bid Z2 = new Bid();
			Z2.NZ = 2;
			Z2.NE = 1;
			Z2.KK = GenKKZ.GenerateValue();
            ev2.ZP = Z2;                                        // передача библиотекаря в событие
            PlanEvent(ev2, 0.0);                          // планирование события 3
            Tracer.PlanEventTrace(ev2);

            #endregion
        }

        //Действия по окончанию прогона
        public override void FinishModelling(int variantCount, int runCount)
        {
            Tracer.AnyTrace("");
            Tracer.TraceOut("==============================================================");
            Tracer.TraceOut("============Статистические результаты моделирования===========");
            Tracer.TraceOut("==============================================================");
            Tracer.AnyTrace("");
            Tracer.TraceOut("Время моделирования: " + string.Format("{0:0.00}", Time));

			//Tracer.TraceOut("Статистические характеристики длины очереди: ");
			//Tracer.TraceOut("МО = " + Variance_LQ.Mx.ToString("#.###"));
			//Tracer.TraceOut("Дисперсия = " + Variance_LQ.Stat.ToString("#.###"));

			//Tracer.TraceOut("");
			//Tracer.TraceOut("KNP = " + KNP);

        }

        //Печать заголовка
        void TraceModelHeader()
        {
            Tracer.TraceOut("==============================================================");
            Tracer.TraceOut("======================= Запущена модель ======================");
            Tracer.TraceOut("==============================================================");
            //вывод заголовка трассировки
            //Tracer.AnyTrace("");
            //Tracer.AnyTrace("Параметры модели:");
            //Tracer.AnyTrace("Интенсивность потока пассажиров:");
            //Tracer.AnyTrace("LAMBD = " + LAMBD );
            //Tracer.AnyTrace("Состояние автобуса:");
            //Tracer.AnyTrace("SA = " + SA    );
            //Tracer.AnyTrace("Размер автобуса:");
            //Tracer.AnyTrace("B = " + B     );
            //Tracer.AnyTrace("Интервал времени прибытия автобуса:");
            //Tracer.AnyTrace("T = " + T     );
            //Tracer.AnyTrace("Погрешность прибытия автобуса:");
            //Tracer.AnyTrace("A = " + A     );
            //Tracer.AnyTrace("Время прогона:");
            //Tracer.AnyTrace("TP = " + TP    );
            //Tracer.AnyTrace("Вариант модели:");
            //Tracer.AnyTrace("NVAR = " + NVAR  );
            //Tracer.AnyTrace("Левая и правая границы числа пассажиров в автобусе:");
            //Tracer.AnyTrace("ml = " + ml    );
            //Tracer.AnyTrace("mp = " + mp    );
            //Tracer.AnyTrace("Левая и правая границы времени высадки пассажира:");
            //Tracer.AnyTrace("bcl = " + bcl   );
            //Tracer.AnyTrace("bcp = " + bcp   );
            //Tracer.AnyTrace("Левая и правая границы времени посадки пассажира:");
            //Tracer.AnyTrace("pcl = " + pcl   );
            //Tracer.AnyTrace("pcp = " + pcp);
            //Tracer.AnyTrace("Левая и правая границы количества выходящих пассажиров из автобуса:");
            //Tracer.AnyTrace("VL = " + VL);
            //Tracer.AnyTrace("VP = " + VP);
            //Tracer.AnyTrace("");

            //Tracer.AnyTrace("Начальное состояние модели:");
            //TraceModel();
            //Tracer.AnyTrace("");

            //Tracer.TraceOut("==============================================================");
            //Tracer.AnyTrace("");
        }

        //Печать строки состояния
        void TraceModel()
        {
			Tracer.AnyTrace(string.Format("TSZ[{0},{1},{2}] KC[{3},{4}] LSQ[{5},{6},{7}] LKPP[{8},{9},{10}]", 
			                              TSZ[0], TSZ[1], TSZ[2], KC[0], KC[1], 
			                              (int)SQ[0].Count, (int)SQ[1].Count, (int)SQ[2].Count, 
			                              (int)KPP[0].Count, (int)KPP[1].Count, (int)KPP[2].Count));
			for (int i = 0; i < KUVS; i++)
			{
				if (KPP[i].Count > 0)
				{
					for (int j = 0; j < KPP[i].Count; j++)
					{
						Console.WriteLine(KPP[i].Count);
						Tracer.AnyTrace("NZ = " + KPP[i][j].Z.NZ + " NE = " + KPP[i][j].Z.NE + " KOK = " + KPP[i][j].Z.KK);
					}
				}
				Console.WriteLine("asdf");
				if (SQ[i].Count > 0)
				{
					for (int j = 0; j < SQ[i].Count; j++)
					{
						Console.WriteLine(SQ[i].Count);
						Tracer.AnyTrace("NZ = " + KPP[i][j].Z.NZ + " NE = " + KPP[i][j].Z.NE + " KOK = " + KPP[i][j].Z.KK);
					}
				}
			}
        }

    }
}

