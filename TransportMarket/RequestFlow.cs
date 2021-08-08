using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StochasticValues;

namespace TransportMarket
{
    /// <summary>
    /// Поток заявок на транспортно-экспедиционное обслуживание (класс)
    /// </summary>  
    public class RequestFlow
    {
        /// <summary>
        /// Субъект транспортного рынка - "хозяин" потока
        /// </summary>
        public MarketSubject Subject;

        protected double simulatedTime;       // время моделирования потока
        protected double flowTime;            // текущее время потока (для суммирования потоков)
        /// <summary>
        /// содержит элементы типа Request:
        /// для экспедитора - входящий поток заявок 
        /// для грузовладельца - исходящий поток заявок 
        /// </summary>
        protected List<Request> requests;

        /// <summary>
        /// аксессор для flowTime
        /// </summary>
        public double FT
        {
            set { if (value >= 0) flowTime = value; }
            get { return flowTime; }
        }

        /// <summary>
        /// Поток заявок. Конструктор класса, позволяющий инициализировать "нулевой" поток заявок
        /// </summary>
        public RequestFlow()
        {
            simulatedTime = flowTime = 0;
            requests = new List<Request>();
        }

        /// <summary>
        /// Поток заявок. Конструктор класса, позволяющий смоделировать поток заявок с заданными характеристиками
        /// </summary>
        /// <param name="st">Модельное время, ч</param>
        /// <param name="Q">Случайная величина объема партии груза</param>
        /// <param name="L">Случайная величина расстояния доставки</param>
        /// <param name="I">Случайная величина интервала поступления заявки</param>
        public RequestFlow(double st, Stochastic sQ, Stochastic sL, Stochastic sI)
        {
            flowTime = 0;
            simulatedTime = st;
            requests = new List<Request>();

            double sumI = 0;
            while (sumI < simulatedTime)
            {
                Request r = new Request(sQ, sL, sI);
                sumI += r.I;
                AddRequest(r);
            }
        }

        /// <summary>
        /// Сортирует заявки в потоке по времени их поступления (необходимо использовать при объединении потоков)
        /// </summary>
        public void SortFlow()
        {
            requests.Sort(new Request.RequestComparer());
            this.RecalculateIntervals();
        }

        /// <summary>
        /// Пересчет интервалов для потока заявок (необходимо использовать при объединении потоков)
        /// </summary>
        protected void RecalculateIntervals()
        {
            if (requests.Count > 0)
                requests[0].I = requests[0].CT;
            if (requests.Count > 1)
                for (int i = 1; i < requests.Count; i++)
                    requests[i].I = requests[i].CT - requests[i - 1].CT;
        }

        /// <summary>
        /// Метод добавляет заявку в конец потока
        /// </summary>
        /// <param name="req">Заявка на транспортно-экспедиционное обслуживание</param>
        public void AddRequest(Request r)
        {
            // определение времени поступление заявки относительно начала моделирования
            if (flowTime == 0) // если заявка в потоке первая, присваиваем времени ее поступления значение интервала
                r.CT = r.I;
            else
                r.CT = GetRequest(requests.Count - 1).CT + r.I;
            flowTime += r.I;
            // непосредственное добавление заявки в поток
            requests.Add(r);
        }
               

        /// <summary>
        /// Возвращает заявку с номером n в последовательном потоке
        /// </summary>
        /// <param name="n">Номер заявки в потоке</param>
        /// <returns>Заявка на транспортное обслуживание</returns>
        public Request GetRequest(int idx)
        {
            if (idx < requests.Count)
                return requests[idx];
            else
                return null; // обработать исключение??
        }

        /// <summary>
        /// Возвращает количество заявок в потоке
        /// </summary>
        /// <returns>Количество заявок в потоке</returns>
        public int GetRequestsNumber() { return requests.Count; }

        /// <summary>
        /// Проверяет, есть ли заявки в потоке
        /// </summary>
        /// <returns>true - если поток заявок пустой, false - если в потоке есть заявки</returns>
        public bool IsEmpty()
        {
            return this.requests.Count == 0;
        }
        
        /// <summary>
        /// Список оптимальных вариантов ЛЦ для данного потока заявок
        /// </summary>
        /// <returns>список оптимальных вариантов с соответсвующими им частотами</returns>
        public SortedList<string, int> OptimalLCvariants()
        {
            SortedList<string, int> optLCs = new SortedList<string, int>();

            if (requests.Count > 0)
                foreach (Request r in requests)
                {
                    string key = r.LC.GetOptimalChain();
                    if (optLCs.ContainsKey(key))
                        optLCs[key]++;
                    else
                        optLCs.Add(key, 1);
                }

            return optLCs;
        }

        /// <summary>
        /// Список оптимальных вариантов ЛЦ для данного потока заявок с заданными доп. параметрами
        /// (доп. параметры устанавливаются для каждой заявки в потоке)
        /// </summary>
        /// <param name="atn">количество грузовых терминалов</param>
        /// <param name="acn">количество таможенных переходов</param>
        /// <param name="it">доступные виды подвозочного транспорта</param>
        /// <param name="lt">доступные виды магистрального транспорта</param>
        /// <returns>список оптимальных вариантов с соответсвующими им частотами</returns>
        public SortedList<string, int> OptimalLCvariants(int atn, int acn, InlandTransport it, LinehaulTransport lt)
        {
            SortedList<string, int> optLCs = new SortedList<string, int>();

            if (requests.Count > 0)
                foreach (Request r in requests)
                {
                    // для каждой заявки потока устанавливаются заданные доп. параметры
                    r.LC = new LogisticsChain(r, atn, acn, it, lt);
                    string key = r.LC.GetOptimalChain();
                    if (optLCs.ContainsKey(key))
                        optLCs[key]++;
                    else
                        optLCs.Add(key, 1);
                }

            return optLCs;
        }

        /// <summary>
        /// Запись потока заявок в файл
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        public void SaveRequestFlow(string FileName)
        {
            TextWriter tw = new StreamWriter(FileName);

            tw.WriteLine("simulatedTime = {0}", simulatedTime);
            tw.WriteLine();
            for (int i = 0; i < requests.Count; i++)
            {
                Request r = requests[i];
                tw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", i, Math.Round(r.CT, 2), Math.Round(r.I, 2),
                                                           Math.Round(r.Q, 2), Math.Round(r.L, 2));
            }
            tw.Close();

        }
    }
}
