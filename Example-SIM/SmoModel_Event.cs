﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonModel.Kernel;

namespace Model_Lab
{
    public partial class SmoModel : Model
    {
        // класс для события 1 - приход пассажира на остановку
        public class K1 : TimeModelEvent<SmoModel>
        {
            #region Атрибуты события
            public Bid ZP;
            #endregion

            // алгоритм обработки события            
            protected override void HandleEvent(ModelEventArgs args)
            {
				for (int NU = 0; NU < KUVS; NU++)
				{
					Console.WriteLine(NU);
					if (Model.KPP[NU].Count != 0)
					{
						Model.KPP[NU][0].Z.KK--;
						if (Model.KPP[NU][0].Z.KK == 0)
						{
							// Переход к следующему этапу
							if (Model.MZ[Model.KPP[NU][0].Z.NZ-1, Model.KPP[NU][0].Z.NE - 1] == -1)
							{
								Model.KPP[NU][0].Z.NE = 1;
							}
							else
							{
								Model.KPP[NU][0].Z.NE++;
							}

                            // Обноление очередей для заявки
							var rec = new QRec();
							Model.GenKKZ.A = Model.MOKK[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1];
							rec.Z = Model.KPP[NU][0].Z;
							rec.Z.KK = Model.GenKKZ.GenerateValue();                  
							if (Model.KPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]-1].Count < Model.MAXKPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]-1])
							{
								Model.KPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]].Add(rec);
							}
							else
							{
								Model.SQ[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]-1].Add(rec);
							}        
							Model.KPP[NU].RemoveAt(0);   
						}
						else
						{
							var rec = new QRec();
							rec.Z = Model.KPP[NU][0].Z;
							Model.KPP[NU].RemoveAt(0);
							Model.KPP[NU].Add(rec);
						}
					}
				}

				var ev1 = new K1(); //создаём объект события                       
				double dt1 = Model.GenTime.GenerateValue();
                Model.PlanEvent(ev1, dt1);

				Model.TraceModel();
            }
        }
    }
}