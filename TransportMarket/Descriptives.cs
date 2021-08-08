using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StochasticValues;

namespace TransportMarket
{

    // описание характеристик субъектов рынка

    /// <summary>
    /// Магистральный транспорт (класс)
    /// </summary>
    public class LinehaulTransport
    {
        /// <summary>
        /// указывает наличие вида транспорта: 0 - автомобильный, 1 - железнодорожный,
        /// 2 - речной, 3 - морской, 4 - воздушный 
        /// </summary>
        bool[] tt = new bool[5];

        public bool Road
        {
            set { tt[0] = value; }
            get { return tt[0]; }
        }
        public bool Railway
        {
            set { tt[1] = value; }
            get { return tt[1]; }
        }
        public bool Riverine
        {
            set { tt[2] = value; }
            get { return tt[2]; }
        }
        public bool Marine
        {
            set { tt[3] = value; }
            get { return tt[3]; }
        }
        public bool Air
        {
            set { tt[4] = value; }
            get { return tt[4]; }
        }

        /// <summary>
        /// Конструктор класса, позволяющий получить доступный транспорт как с.в. 
        /// </summary>
        /// <param name="isStochastic">параметр конструктора: true - задается как с.в., false - доступны все виды транспорта</param>
        public LinehaulTransport(bool isStochastic)
        {
            if (isStochastic)
            {
                Stochastic r = new Stochastic(Stochastic.RECT, 0, 10, 0);
                for (int i = 0; i < tt.Length; i++)
                {
                    if (r.GetValue() > 5) tt[i] = false;
                    else tt[i] = true;
                }
                // проверка на случай, для всех элементов сгенерировалось значение false;
                // в таком случае назначить доступным только автомобильный транспорт
                if (!tt[0] && !tt[1] && !tt[2] && !tt[3] && !tt[4]) tt[0] = true;
            }
            else
                tt[0] = tt[1] = tt[2] = tt[3] = tt[4] = true;
        }

        public LinehaulTransport(int[] a)
        {
            for (int i = 0; i < tt.Length; i++)
            {
                if (i < a.Length)
                    tt[i] = (a[i] != 0);
                else
                    tt[i] = false;
            }
        }

        public LinehaulTransport(string s)
        {
            for (int i = 0; i < tt.Length; i++)
            {
                if (i < s.Length)
                    tt[i] = !(s[i] == '0');
                else
                    tt[i] = false;
            }
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="lt">Межтерминальный транспорт</param>
        public LinehaulTransport(LinehaulTransport lt)
        {
            if (lt.Road) this.tt[0] = true; else this.tt[0] = false;
            if (lt.Railway) this.tt[1] = true; else this.tt[1] = false;
            if (lt.Riverine) this.tt[2] = true; else this.tt[2] = false;
            if (lt.Marine) this.tt[3] = true; else this.tt[3] = false;
            if (lt.Air) this.tt[4] = true; else this.tt[4] = false;
        }

    }

    /// <summary>
    /// Подвозочный транспорт (класс)
    /// </summary>
    public class InlandTransport
    {
        /// <summary>
        /// указывает наличие вида транспорта: 0 - автомобильный, 1 - железнодорожный, 2 - речной
        /// </summary>
        bool[] tt = new bool[3];

        public bool Road
        {
            set { tt[0] = value; }
            get { return tt[0]; }
        }
        public bool Railway
        {
            set { tt[1] = value; }
            get { return tt[1]; }
        }
        public bool Riverine
        {
            set { tt[2] = value; }
            get { return tt[2]; }
        }

        public InlandTransport(bool isStochastic)
        {
            if (isStochastic)
            {
                Stochastic r = new Stochastic(Stochastic.RECT, 0, 10, 0);
                for (int i = 0; i < tt.Length; i++)
                {
                    if (r.GetValue() > 5) tt[i] = false;
                    else tt[i] = true;
                }
                // проверка на случай, для всех элементов сгенерировалось значение false;
                // в таком случае назначить доступным только автомобильный транспорт
                if (!tt[0] && !tt[1] && !tt[2]) tt[0] = true;
            }
            else
                tt[0] = tt[1] = tt[2] = true;
        }

        public InlandTransport(int[] a)
        {
            for (int i = 0; i < tt.Length; i++)
            {
                if (i < a.Length)
                    tt[i] = (a[i] != 0);
                else
                    tt[i] = false;
            }
        }

        public InlandTransport(string s)
        {
            for (int i = 0; i < tt.Length; i++)
            {
                if (i < s.Length)
                    tt[i] = !(s[i] == '0');
                else
                    tt[i] = false;
            }
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="lt">Немагистральный транспорт</param>
        public InlandTransport(InlandTransport it)
        {
            if (it.Road) this.tt[0] = true; else this.tt[0] = false;
            if (it.Railway) this.tt[1] = true; else this.tt[1] = false;
            if (it.Riverine) this.tt[2] = true; else this.tt[2] = false;
        }

    }

    /// <summary>
    /// Обслуженная заявка (класс)
    /// </summary>
    public class RequestServiced : Request
    {
        /// <summary>
        /// Время налача обслуживания
        /// </summary>
        public double beginTime;
        /// <summary>
        /// Время окончания обслуживания
        /// </summary>
        public double endTime;

        //public Dispatcher dispatcher;

        public RequestServiced(Request r)
        {
            this.Q = r.Q;
            this.L = r.L;
            this.I = r.I;
            this.CT = r.CT;

            this.availableIT = r.availableIT;
            this.availableLT = r.availableLT;
            this.CNum = r.CNum;
            this.TNum = r.TNum;
            this.LC = r.LC;
            //dispatcher = null;
        }

    }

    /// <summary>
    /// Грузовой автомобиль (класс)
    /// </summary>
    public class Automobile
    {
        string model;   // название модели автомобиля
        byte bodyType;  // тип кузова:
        // 1 - легковой
        // 2 - легковой 
        // 3 - бортовой
        // 4 - тягач
        // 5 - самосвал
        // 7 - фургон

        double capacity;    // грузоподъемность, т
        double bodySpace;   // объем кузова, м3

        double costOf1km;   // переменная составляющая себестоимости, $/км
        double costOf1hour; // постоянная составляющая себестоимости, $/ч

        public Automobile()
        {
            model = "Unknown";
            bodyType = 7;
            capacity = 20;
            bodySpace = 86;
            costOf1hour = 25;
            costOf1km = 2;
        }
    }

    /// <summary>
    /// Парк подвижного состава (класс)
    /// </summary>
    public class Fleet
    {
        /// <summary>
        /// Субъект рынка ТЭУ - "хозяин" парка транспортных средств
        /// </summary>
        MarketSubject Subject;
        List<Automobile> automobiles; // содержит элементы типа Automobile

        public Fleet()
        {
            automobiles = new List<Automobile>();
        }

        public Fleet(List<Automobile> fl)
        {
            automobiles = new List<Automobile>(fl);
        }

        /// <summary>
        /// Возвращает количество автомобилей в структуре парка
        /// </summary>
        /// <returns>Количество автомобилей в парке</returns>
        public int Count() { return automobiles.Count; }
    }

    /// <summary>
    /// Складское хозяйство (класс)
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// Субъект рынка ТЭУ - "хозяин" складского хозяйства
        /// </summary>
        MarketSubject Subject;
        /// <summary>
        /// количество постов погрузки-разгрузки
        /// </summary>
        int postsNumber
        {
            get { return productivity.Length; }
        }
        /// <summary>
        /// производительность постов погрузки-разгрузки, т/ч
        /// </summary>
        double[] productivity;
        /// <summary>
        /// складская поверхность, м2
        /// </summary>
        public double storageSquare;

        /// <summary>
        /// Стоимость 1 ч работ по перегрузке партии товара, $/ч
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double ReloadCosts;
        /// <summary>
        /// Cтоимость 1 ч работ по формированию и расформированию партии товара, $/ч
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PackWorkerCosts;
        /// <summary>
        /// Стоимость 1 ч хранения 1 т груза на грузовом терминале, $/(т∙ч) 
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double StorageCosts;

        /// <summary>
        /// Продолжительность погрузки 1 т товара с использованием мощностей терминала, ч/т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public Stochastic LoadTime;
        /// <summary>
        /// Продолжительность разгрузки 1 т товара с использованием мощностей терминала, ч/т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public Stochastic UnloadTime;
        /// <summary>
        /// Время расформирования и формирования пакета на 1 т товара, ч/т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public Stochastic PackFormTime;

    }

    /// <summary>
    /// Диспетчер, менеджер-экспедитор (класс)
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// Имя диспетчера
        /// </summary>
        public String name;
        /// <summary>
        /// Удельные затраты на 1 час работы диспетчера, $/ч
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double HourCosts;

        /// <summary>
        /// Производительность труда, заявка/ч 
        /// </summary>
        public double efficiency;

        public List<RequestServiced> reqServiced;

        private bool isBusy;
        /// <summary>
        /// time when dispatcher become free (not busy)
        /// </summary>
        private double timeFree;

        //int age; // возраст
        //int experience; // опыт работы, лет

        /// <summary>
        /// Диспетчер (конструктор класса)
        /// </summary>
        public Dispatcher()
        {
            HourCosts = 10;
            efficiency = 1;
        }

        /*
        public synchronized void setTimeFree(double val)
        {
            if (isBusy) try {wait();} catch(Exception e){}
            if (val>=0) timeFree = val;
        }
        */
        public bool getBusy() { return isBusy; }

        /*
        synchronized public void checkBusy(Request r)
        {
            //if(!timeIsChanged)
            try {wait(1);} catch(Exception e){}
		
            isBusy = (r.apT < timeFree) ? true : false;
            //timeIsChanged = false;
            //notify();
            System.out.println("For request " + Math.round(r.apT) + " " + name + " is busy: " + isBusy);
            //if (!isBusy) new ServiceRequest(r,this).start();
        }
	
        public void serveRequest(Request r)
        {
            new ServiceRequest(r,this).start();
        }
        */

    }

}

