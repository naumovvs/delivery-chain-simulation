using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StochasticValues;

namespace TransportMarket
{
    /// <summary>
    /// Численные параметры модели рынка транспортных услуг
    /// </summary>
    public class NumericParameters
    {
        /// <summary>
        /// Cтавка налога на прибыль, %
        /// </summary>
        public int ProfitTaxRate;
        /// <summary>
        /// Cтавка налога на добавленную стоимость, %
        /// </summary>
        public int VATRate;
        /// <summary>
        /// Коэффициент, учитывающий потери вследствие замораживания денежных средств
        /// при осуществлении доставки партии груза, %/год
        /// (среднее значения годовой депозитной ставки, предлагаемой банками в регионе грузоотправителя)
        /// </summary>
        public int DepositeRate;
        /// <summary>
        /// Cтавка таможенного сбора, %
        /// </summary>
        public int DutyRate;
        /// <summary>
        /// Cтавка ввозной пошлины, %
        ///  до 10 тыс.дол. - бесплатно; более 10 тыс.дол. - 10%
        /// </summary>
        public int ImportRate;

        // Параметры для грузовладельца

        /// <summary>
        /// Номинальная грузоподъемность транспортного пакета, т
        /// </summary>
        public double PacketCapacity;
        /// <summary>
        /// Коэффициент, учитывающий оборачиваемость транспортировочных емкостей.
        /// Если емкость не возвращается грузоотправителю, то К = 1;
        /// если договор доставки предусматривает возврат грузовой емкости, то К является величиной,
        /// обратной среднему количеству оборотов грузовой емкости до списания;
        /// если стоимость транспортировочной емкости включена в стоимость товара для грузополучателя, то К = 0.
        /// </summary>
        public double TurnoverCoefficient;
        /// <summary>
        /// Стоимость упаковочных материалов, $/пакет; 
        /// </summary>
        public double PackageMaterialsCosts;
        /// <summary>
        /// Стоимость транспортировочной емкости, $/пакет;
        /// </summary>
        public double PackageCosts;
        /// <summary>
        /// Cтоимость 1 т товара, $/т
        /// (данное поле есть также в классе FreightOwner)
        /// </summary>
        public double CargoCosts;
        /// <summary>
        /// Cтоимость 1 ч работы работника, формирующего транспортные пакеты, $/ч
        /// (для грузовладельца)
        /// </summary>
        public double FOPackWrokerCosts;
        /// <summary>
        /// Время на формирование одного транспортного пакета (для грузовладельца), ч/пакет
        /// </summary>
        public Stochastic FOPackFormTime;
        /// <summary>
        /// Тариф подрядчика по осуществлению ПРР, $/т
        /// количество операций по погрузке-разгрузке партии товара
        /// NLU=atn+2 (в таможенных пунктах предполагается прямая перевалка)
        /// </summary>
        public double LUTariff;

        // Параметры работы экспедитора

        /// <summary>
        /// Время на обработку последней поступившей заявки, ч
        /// (поле присутствует также в классе FreightForwarder)
        /// </summary>
        public double FFReqOpTime;
        /// <summary>
        /// Количество трудоустроенных менеджеров (диспетчеров)
        /// (поле присутствует также в классе FreightForwarder)
        /// </summary>
        public int FFDispatchersNumber;
        /// <summary>
        /// Удельные затраты на 1 час работы диспетчера, $/ч
        /// (поле присутствует также в классе FreightForwarder)
        /// </summary>
        public double FFDispatcher1HourCosts;
        /// <summary>
        /// Составляющая себестоимости часа работы экспедитора, включающая услуги сторонних организаций
        /// (поле присутствует также в классе FreightForwarder)
        /// </summary>
        public double FFPaid1HourCosts;
        /// <summary>
        /// Уровень рентабельности экспедитора
        /// (поле присутствует также в классе FreightForwarder)
        /// </summary>
        public double FFProfitability;

        // Параметры работы грузового терминала

        /// <summary>
        /// Стоимость 1 ч работ по перегрузке партии товара, $/ч 
        /// </summary>
        public double FTReloadCosts;
        /// <summary>
        /// Cтоимость 1 ч работ по формированию и расформированию партии товара, $/ч
        /// </summary>
        public double FTPackWorkerCosts;
        /// <summary>
        /// Стоимость 1 ч хранения 1 т груза на грузовом терминале, $/(т∙ч) 
        /// </summary>
        public double FTStorageCosts;
        /// <summary>
        /// Продолжительность погрузки 1 т товара с использованием мощностей терминала, ч/т
        /// </summary>
        public Stochastic FTLoadTime;
        /// <summary>
        /// Продолжительность разгрузки 1 т товара с использованием мощностей терминала, ч/т 
        /// </summary>
        public Stochastic FTUnloadTime;
        /// <summary>
        /// Время расформирования и формирования пакета на 1 т товара, ч/т 
        /// </summary>
        public Stochastic FTPackFormTime;
        /// <summary>
        /// Время промежуточного хранения партии груза на складе грузового терминала, ч 
        /// </summary>
        public Stochastic FTStorageTime;
        /// <summary>
        /// Доля стоимости приобретаемых услуг и товаров в себестоимости услуг терминала 
        /// </summary>
        public double FTExpensesPaid;
        /// <summary>
        /// Общий тариф на услуги терминала, $/т
        /// </summary>
        public double FTTariff;

        // Параметры работы видов транспорта

        /// <summary>
        /// Коэффициенты неравномерности пути для видов транспорта 
        /// </summary>
        public double[] k = { 1.2, 1.18, 1.4, 1.1, 1.18, 1.35, 1, 1 };
        /// <summary>
        /// Приоритет магистрального транспорта при распределении расстояния
        /// </summary>
        public double LinehaulPriority = 0.9;

        /// <summary>
        /// Средние значения для автомобильного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams ITroad;
        /// <summary>
        /// Средние значения для железнодорожного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams ITrailway;
        /// <summary>
        /// Средние значения для речного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams ITriverine;
        /// <summary>
        /// Средние значения для автомобильного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams LTroad;
        /// <summary>
        /// Средние значения для железнодорожного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams LTrailway;
        /// <summary>
        /// Средние значения для речного транспорта (подвозочный)
        /// </summary>
        public TransportTypeParams LTriverine;
        /// <summary>
        /// Средние значения для морского транспорта
        /// </summary>
        public TransportTypeParams LTmarine;
        /// <summary>
        /// Средние значения для воздушного транспорта
        /// </summary>
        public TransportTypeParams LTair;

        /// <summary>
        /// Продолжительность простоя в таможенном пункте, ч
        /// </summary>
        public Stochastic CustomsDetention;

        /// <summary>
        /// Численные параметры по умолчанию
        /// </summary>
        public NumericParameters()
        {
            // Ставки налогов и сборов
            ProfitTaxRate = 30;
            VATRate = 20;
            DepositeRate = 20;
            DutyRate = 5;
            ImportRate = 10;

            // Параметры грузовладельца
            CargoCosts = 1000;
            PacketCapacity = 20;
            TurnoverCoefficient = 0;
            PackageMaterialsCosts = 0;
            PackageCosts = 1000;
            FOPackWrokerCosts = 5;
            FOPackFormTime = new Stochastic(1, 0.5, 0.1, 0);
            LUTariff = 0.5;

            // Параметры экспедитора
            FFReqOpTime = 0.9;
            FFDispatchersNumber = 2;
            FFDispatcher1HourCosts = 10;
            FFPaid1HourCosts = 6;
            FFProfitability = 0.1;

            // Параметры терминала
            FTReloadCosts = 10;
            FTPackWorkerCosts = 5;
            FTStorageCosts = 0.1;
            FTLoadTime = new Stochastic(1, 0.05, 0.01, 0);
            FTUnloadTime = new Stochastic(1, 0.05, 0.01, 0);
            FTPackFormTime = new Stochastic(1, 0.2, 0.05, 0);
            FTStorageTime = new Stochastic(1, 24, 8, 0);
            FTExpensesPaid = 0.3;
            FTTariff = 10;

            // Параметры видов транспорта
            ITroad = new TransportTypeParams(1, 10, 0.5, 0.1);
            ITrailway = new TransportTypeParams(2, 7, 0.2, 0.05);
            ITriverine = new TransportTypeParams(3, 500, 0.01, 0.01);
            LTroad = new TransportTypeParams(4, 15, 0.3, 0.08);
            LTrailway = new TransportTypeParams(5, 5, 0.15, 0.04);
            LTriverine = new TransportTypeParams(6, 1000, 5, 0.05);
            LTmarine = new TransportTypeParams(7, 5000, 5, 0.001);
            LTair = new TransportTypeParams(8, 2000, 1, 1);

            CustomsDetention = new Stochastic(1, 10, 3, 0);
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="np">численные параметры</param>
        public NumericParameters(NumericParameters np)
        {

            this.k = np.k;
            this.LinehaulPriority = np.LinehaulPriority;

            // Ставки налогов и сборов
            this.ProfitTaxRate = 30;
            this.VATRate = 20;
            this.DepositeRate = 20;
            this.DutyRate = 5;
            this.ImportRate = 10;

            // Параметры грузовладельца
            this.CargoCosts = 1000;
            this.PacketCapacity = 20;
            this.TurnoverCoefficient = 0;
            this.PackageMaterialsCosts = 0;
            this.PackageCosts = 1000;
            this.FOPackWrokerCosts = 5;
            this.FOPackFormTime = new Stochastic(1, 0.5, 0.1, 0);
            this.LUTariff = 0.5;

            // Параметры экспедитора
            this.FFReqOpTime = 0.5;
            this.FFDispatchersNumber = 1;
            this.FFDispatcher1HourCosts = 10;
            this.FFPaid1HourCosts = 6;
            this.FFProfitability = 0.1;

            // Параметры терминала
            this.FTReloadCosts = 10;
            this.FTPackWorkerCosts = 5;
            this.FTStorageCosts = 0.1;
            this.FTLoadTime = new Stochastic(1, 0.05, 0.01, 0);
            this.FTUnloadTime = new Stochastic(1, 0.05, 0.01, 0);
            this.FTPackFormTime = new Stochastic(1, 0.2, 0.05, 0);
            this.FTStorageTime = new Stochastic(1, 24, 8, 0);
            this.FTExpensesPaid = 0.3;
            this.FTTariff = 10;

            // Параметры видов транспорта
            this.ITroad = new TransportTypeParams(1, 10, 0.5, 0.1);
            this.ITrailway = new TransportTypeParams(2, 7, 0.2, 0.05);
            this.ITriverine = new TransportTypeParams(3, 500, 0.01, 0.01);
            this.LTroad = new TransportTypeParams(4, 15, 0.3, 0.08);
            this.LTrailway = new TransportTypeParams(5, 5, 0.15, 0.04);
            this.LTriverine = new TransportTypeParams(6, 1000, 5, 0.05);
            this.LTmarine = new TransportTypeParams(7, 5000, 5, 0.001);
            this.LTair = new TransportTypeParams(8, 2000, 1, 1);

            this.CustomsDetention = new Stochastic(1, 10, 3, 0);

        }

    }

    /// <summary>
    /// Параметры типа транспорта
    /// </summary>
    public class TransportTypeParams
    {
        /// <summary>
        /// Возвращает номер вида транспорта:
        /// 1 - автомобильный подвозочный
        /// 2 - железнодорожный подвозочный
        /// 3 - речной подвозочный
        /// 4 - автомобильный магистральный
        /// 5 - железнодорожный магистральный
        /// 6 - речной магистральный
        /// 7 - морской магистральный
        /// 8 - воздушный магистральный
        /// </summary>
        public byte Type;

        /// <summary>
        /// Постоянная составляющая себестоимости транспортировки, $/ч
        /// </summary>
        public double Costs1Hour;
        /// <summary>
        /// Переменная составляющая себестоимости транспортировки, $/км
        /// </summary>
        public double Costs1Km;
        /// <summary>
        /// Тариф на услуги перевозчика, $/ткм
        /// </summary>
        public double Tariff;

        /// <summary>
        /// Возвращает номер вида транспорта:
        /// 1 - автомобильный подвозочный
        /// 2 - железнодорожный подвозочный
        /// 3 - речной подвозочный
        /// 4 - автомобильный магистральный
        /// 5 - железнодорожный магистральный
        /// 6 - речной магистральный
        /// 7 - морской магистральный
        /// 8 - воздушный магистральный
        /// </summary>
        //public byte Type
        //{
        //    get { return type; }
        //}

        /// <summary>
        /// Cредняя скорость движения транспортного средства, км/ч
        /// </summary>
        public Stochastic Velocity;
        /// <summary>
        /// Продолжительность погрузки 1 т груза, ч/т
        /// </summary>
        public Stochastic LoadDuration;
        /// <summary>
        /// Продолжительность разгрузки 1 т груза, ч/т
        /// </summary>
        public Stochastic UnloadDuration;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="type">Тип вида транспорта: 1..8</param>
        /// <param name="c1h">Постоянная составляющая себестоимости доставки, $/ч</param>
        /// <param name="c1km">Переменная составляющая себестоимости доставки, $/км</param>
        /// <param name="t">Тариф на перевозку, $/ткм</param>
        public TransportTypeParams(byte type, double c1h, double c1km, double t)
        {
            this.Type = type;
            Costs1Hour = c1h;
            Costs1Km = c1km;
            Tariff = t;

            switch (type)
            {
                case 1: // автомобильный подвозочный
                    Velocity = new Stochastic(1, 60, 15, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 2: // железнодорожный подвозочный
                    Velocity = new Stochastic(1, 50, 5, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 3: // речной подвозочный
                    Velocity = new Stochastic(1, 30, 5, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 4: // автомобильный магистральный
                    Velocity = new Stochastic(1, 70, 10, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 5: // железнодорожный магистральный
                    Velocity = new Stochastic(1, 70, 5, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 6: // речной магистральный
                    Velocity = new Stochastic(1, 30, 5, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 7: // морской магистральный
                    Velocity = new Stochastic(1, 40, 5, 0);
                    LoadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    UnloadDuration = new Stochastic(1, 0.05, 0.01, 0);
                    break;
                case 8: // воздушный магистральный
                    Velocity = new Stochastic(1, 750, 15, 0);
                    LoadDuration = new Stochastic(1, 0.2, 0.05, 0);
                    UnloadDuration = new Stochastic(1, 0.2, 0.05, 0);
                    break;
            }
        }


        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="ttp">параметры вида транспорта</param>
        public TransportTypeParams(TransportTypeParams ttp)
        {
            this.Type = ttp.Type;
            this.Costs1Hour = ttp.Costs1Hour;
            this.Costs1Km = ttp.Costs1Km;
            this.Tariff = ttp.Tariff;

            this.Velocity = new Stochastic(ttp.Velocity);
            this.LoadDuration = new Stochastic(ttp.LoadDuration);
            this.UnloadDuration = new Stochastic(ttp.UnloadDuration);

        }


    }
}
