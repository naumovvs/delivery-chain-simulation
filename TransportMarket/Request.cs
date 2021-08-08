using System.Collections.Generic;
using StochasticValues;

namespace TransportMarket
{
    /// <summary>
    /// Заявка на транспортное обслуживание (класс)
    /// </summary>
    public class Request
    {
        /// <summary>
        /// объем партии груза, т
        /// </summary>
        double q;
        /// <summary>
        /// расстояние доставки, км
        /// </summary>
        double l;
        /// <summary>
        /// интервал поступления заявки относительно предыдущей, ч
        /// </summary>
        double i;

        // дополнительные параметры заявки 
        /// <summary>
        /// доступный подвозочный транспорт
        /// </summary>
        public InlandTransport availableIT;
        /// <summary>
        /// доступный магистральный транспорт
        /// </summary>
        public LinehaulTransport availableLT;
        /// <summary>
        /// доступное количество терминалов
        /// </summary>
        int atn;
        /// <summary>
        /// количество таможенных переходов
        /// </summary>
        int acn;

        /// <summary>
        /// текущее время поступления заявки в потоке относительно начала моделирования, ч
        /// </summary>
        double ct;

        /// <summary>
        /// Логистическая цепочка, реализующая заявку
        /// </summary>
        public LogisticsChain LC;

        public double Q
        {
            get { return q; }
            set { if (value >= 0) q = value; }
        }
        public double L
        {
            get { return l; }
            set { if (value >= 0) l = value; }
        }
        public double I
        {
            get { return i; }
            set { if (value >= 0) i = value; }
        }
        public double CT
        {
            get { return ct; }
            set { if (value >= 0) ct = value; }
        }

        /// <summary>
        /// Аксессор поля, содержащего доступное количество терминалов
        /// Количество теминалов находится в интервале [0..2]
        /// </summary>
        public int TNum
        {
            set { if (value > 0 && value < 3) atn = value; }
            get { return atn; }
        }
        /// <summary>
        /// Аксессор поля, содержащего количество таможенных переходов
        /// Количество таможенных переходов находится в интервале [0..2]
        /// </summary>
        public int CNum
        {
            set { if (value > 0 && value < 3) acn = value; }
            get { return acn; }
        }

        /// <summary>
        /// Заявка на транспортное обслуживание (конструктор класса по умолчанию) 
        /// </summary>
        public Request()
        {
            // генерация доп.параметров по умолчанию
            availableIT = new InlandTransport(true);
            availableLT = new LinehaulTransport(true);
            atn = (int)(new Stochastic(Stochastic.RECT, loc: 0, scl: 3)).GetValue();
            acn = (int)(new Stochastic(Stochastic.RECT, loc: 0, scl: 3)).GetValue();

            LC = new LogisticsChain(this);
        }

        /// <summary>
        /// Заявка на транспортное обслуживание (конструктор класса)
        /// </summary>
        /// <param name="sQ"> Случайная величина партии груза (т) </param>
        /// <param name="sL"> Случайная величина расстояния доставки (км) </param>
        /// <param name="sI"> Случайная величина интервала поступления заявок (ч) </param>
        public Request(Stochastic sQ, Stochastic sL, Stochastic sI)
        {
            q = sQ.GetValue();
            l = sL.GetValue();
            i = sI.GetValue();

            // генерация доп.параметров
            availableIT = new InlandTransport(true);
            availableLT = new LinehaulTransport(true);
            atn = (int)(new Stochastic(Stochastic.RECT, loc: 0, scl: 3)).GetValue();
            acn = (int)(new Stochastic(Stochastic.RECT, loc: 0, scl: 3)).GetValue();

            LC = new LogisticsChain(this);
        }

        /// <summary>
        /// Устанавливает дополнительные параметры заявки
        /// </summary>
        /// <param name="atn">доступное количество грузовых терминалов</param>
        /// <param name="acn">количество таможенных переходов</param>
        /// <param name="it">доступный подвозочный транспорт</param>
        /// <param name="lt">доступный магистральный транспорт</param>
        public void SetAdditionalParams(int atn, int acn, InlandTransport it, LinehaulTransport lt)
        {
            if (atn > 0) this.atn = atn;
            if (acn >= 0) this.acn = acn;
            availableIT = it; availableLT = lt;
        }

        /// <summary>
        /// Объект для сравнения двух заявок, наследует интерфейс IComparer
        /// В качестве критерия сравнения задается поле ct - время поступления заявки
        /// </summary>
        public class RequestComparer : IComparer<Request>
        {
            public int Compare(Request x, Request y)
            {
                return (x.ct).CompareTo(y.ct);
            }
        }
    }
    
}
