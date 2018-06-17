using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommonModel.StatisticsCollecting;
using CommonModel.RandomStreamProducing;
using CommonModel.Collections;
using CommonModel.Kernel;

namespace Model_Lab
{

    public partial class SmoModel : Model
    {

        #region Параметры модель

        // Количество узлов
        const int KUVS = 3;
        // Количество заявок
        const int KZ = 2;
        // Маршрут
        int[,] MZ = new int[KZ, KUVS];
        // Математическое ожидание
        int[,] MOKK = new int[KZ, KUVS];
        // Максимальное количество ПП
        int[] MAXKPP = new int[KUVS];
        // Время прогона
        double TP;

        int TK = 0;
        #endregion

        #region Переменные состояния модели

        double[] TSZ = { 0, 0, 0 };
        double[] KC = { 0, 0 };
        TIntVar[] LKPP;
        TIntVar[] LSQ;

        #endregion

        #region Дополнительные структуры

        // Заявки в узлах ВС
        public class Bid
        {
            // Номер заявки
            public int NZ;
            // Номер этапа
            public int NE;
            // Требуемое количество квантов
            public int KK;
        }

        // Элемент очереди заявки в узлах ВС 
        class QRec : QueueRecord
        {
            public Bid Z;
        }

        // Группа очередей ПП
        SimpleModelList<QRec>[] SQ;
        // Группа внешних очередей
        SimpleModelList<QRec>[] KPP;

        #endregion

        #region Cборщики статистики

        // 	Интенсивность числа полных циклов
        Variance<int>[] Variance_LKPP;
        Variance<int>[] Variance_LSQ;

        #endregion

        #region Генераторы ПСЧ

        // Генератор времени появления пассажиров
        PoissonStream GenKKZ;
        // Генератор времени прибытия автобуса на остановку
		ExpStream GenTime;
        
        #endregion

        #region Инициализация объектов модели

        public SmoModel(Model parent, string name)
            : base(parent, name)
        {
            LKPP = InitModelObjectArray<TIntVar>(3, "сборщик времени выполнения ТП_#");
            LSQ = InitModelObjectArray<TIntVar>(3, "сборщик времени выполнения ТП_#");
            SQ = InitModelObjectArray<SimpleModelList<QRec>>(KUVS, "Внешняя очередь");
			KPP = InitModelObjectArray<SimpleModelList<QRec>>(KUVS, "Очередь ПП");

			GenKKZ = InitModelObject<PoissonStream>();
			GenTime = InitModelObject<ExpStream>();
            Variance_LKPP = InitModelObjectArray<Variance<int>>(3, "сборщик времени выполнения ТП_#");
            Variance_LSQ = InitModelObjectArray<Variance<int>>(3, "сборщик времени выполнения ТП_#");
            Variance_LKPP[0].ConnectOnSet(LKPP[0]);
            Variance_LKPP[1].ConnectOnSet(LKPP[1]);
            Variance_LKPP[2].ConnectOnSet(LKPP[2]);
            Variance_LSQ[0].ConnectOnSet(LSQ[0]);
            Variance_LSQ[1].ConnectOnSet(LSQ[1]);
            Variance_LSQ[2].ConnectOnSet(LSQ[2]);
        }

        #endregion
    }
}
