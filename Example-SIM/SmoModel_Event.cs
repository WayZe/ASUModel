using System;
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
                Model.Tracer.AnyTrace(Model.TK + "\tK1");
                Model.TK++;
                for (int NU = 0; NU < KUVS; NU++)
				{
					if (Model.KPP[NU].Count != 0)
					{
                        Model.KPP[NU][0].Z.KK--;
                        Model.TSZ[NU]++;
						if (Model.KPP[NU][0].Z.KK == 0)
						{
							// Переход к следующему этапу
							if (Model.MZ[Model.KPP[NU][0].Z.NZ-1, Model.KPP[NU][0].Z.NE] == -1)
							{
								Model.KPP[NU][0].Z.NE = 1;
                                Model.KC[Model.KPP[NU][0].Z.NZ-1]++;
							}
							else
							{
								Model.KPP[NU][0].Z.NE++;
							}

                            // Обноление очередей для заявки
                            var rec = new QRec();
							Model.GenKKZ.A = Model.MOKK[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1];
							rec.Z = Model.KPP[NU][0].Z;
                            do
                                rec.Z.KK = Model.GenKKZ.GenerateValue();
                            while (rec.Z.KK <= 0);

                            if ((int)Model.KPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]].Count < (int)Model.MAXKPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]])
							{
                                Model.KPP[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]].Add(rec);
							}
							else
							{
                                Model.SQ[Model.MZ[Model.KPP[NU][0].Z.NZ - 1, Model.KPP[NU][0].Z.NE - 1]].Add(rec);
                            }        
							Model.KPP[NU].RemoveAt(0);

                            if (Model.SQ[NU].Count > 0 && Model.KPP[NU].Count < Model.MAXKPP[NU])
                            {
                                Model.KPP[NU].Add(Model.SQ[NU][0]);
                                Model.SQ[NU].RemoveAt(0);
                            }
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
                //Model.Tracer.PlanEventTrace(ev1);
                Model.Tracer.AnyTrace("+" + Model.TK + "\tK1");

                Model.LKPP[0].Value = Model.KPP[0].Count;
                Model.LKPP[1].Value = Model.KPP[1].Count;
                Model.LKPP[2].Value = Model.KPP[2].Count;

                Model.LSQ[0].Value = Model.SQ[0].Count;
                Model.LSQ[1].Value = Model.SQ[1].Count;
                Model.LSQ[2].Value = Model.SQ[2].Count;

                Model.TraceModel();
            }
        }
    }
}
