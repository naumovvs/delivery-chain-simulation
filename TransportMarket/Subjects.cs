using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StochasticValues;

namespace TransportMarket
{
    /// <summary>
    /// Субъект рынка транспортно-экспедиционных услуг
    /// </summary>
    public abstract class MarketSubject
    {
        /// <summary>
        /// Имя субъекта транспортного рынка
        /// </summary>
        public string Name;
        /// <summary>
        /// расположение субъекта транспортного рынка
        /// </summary>
        public Location Loc;

        /// <summary>
        /// Возвращает входящий/исходящий поток заявок субъекта рынка
        /// </summary>
        /// <returns>Поток заявок</returns>
        public abstract RequestFlow GetRequestFlow();
    }

    /// <summary>
    /// Перевозчик (класс)
    /// </summary>
    public class Carrier : MarketSubject
    {
        TransportTypeParams ttp;
        RequestFlow reqFlow;
        public Fleet fleet;


        public override RequestFlow GetRequestFlow() { return reqFlow; }
    }

    /// <summary>
    /// Экспедитор (класс)
    /// </summary>
    public class FreightForwarder : MarketSubject
    {
        /// <summary>
        /// Диспетчеры предприятия
        /// </summary>
        public List<Dispatcher> Staff;
        /// <summary>
        /// Парк подвижного состава
        /// </summary>
        public Fleet Fleet;
        /// <summary>
        /// Складское хозяйство
        /// </summary>
        public Warehouse Warehouse;

        /// <summary>
        /// Составляющая себестоимости часа работы экспедитора, включающая услуги сторонних организаций
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PaidCosts;
        /// <summary>
        /// Уровень рентабельности экспедитора
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double Profitability;
        /// <summary>
        /// Количество трудоустроенных менеджеров (диспетчеров)
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public int DispatchersNumber
        {
            get { return Staff.Count; }
        }

        /// <summary>
        /// Время на обработку последней поступившей заявки, ч // ??? является характеристикой отдельной заявки?
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double ReqOpTime;
        /// <summary>
        /// Входящий поток заявок
        /// </summary>
        protected RequestFlow reqFlow;

        /// <summary>
        /// Конструктор класса по-умолчанию
        /// </summary>
        public FreightForwarder()
        {
            Name = "Unknown Freight Forwarder";
            //Loc = new Location();

            Staff = new List<Dispatcher>();
            Staff.Add(new Dispatcher());
            Fleet = new Fleet();

            reqFlow = new RequestFlow();
        }

        /// <summary>
        /// Конструктор класса с заданным штатом диспетчеров
        /// </summary>
        /// <param name="st">Список диспетчеров (менеджеров-логистов)</param>
        public FreightForwarder(List<Dispatcher> st)
        {
            Name = "Unknown Freight Forwarder";
            //Loc = new Location();

            Staff = new List<Dispatcher>(st);
            Fleet = new Fleet();

            reqFlow = new RequestFlow();
        }

        /// <summary>
        /// Конструктор класса с заданным штатом диспетчеров и парком грузовых автомобилей
        /// </summary>
        /// <param name="st">Список менеджеров-логистов</param>
        /// <param name="fl">Список автомобилей</param>
        public FreightForwarder(List<Dispatcher> st, List<Automobile> fl)
        {
            Name = "Unknown Freight Forwarder";
            //Loc = new Location();

            Staff = new List<Dispatcher>(st);
            Fleet = new Fleet(fl);

            reqFlow = new RequestFlow();
        }

        public override RequestFlow GetRequestFlow() { return reqFlow; }

        /// <summary>
        /// Устанавливает поток заявок из аргумента
        /// </summary>
        /// <param name="rf">Поток заявок</param>
        public void SetRequestFlow(RequestFlow rf) { reqFlow = rf; }

        /// <summary>
        /// Генерирует входящий поток заявок с заданными характеристиками
        /// </summary>
        /// <param name="st">Время моделирования, ч</param>
        /// <param name="Q">с.в. партии груза (т)</param>
        /// <param name="L">с.в. расстояния доставки (км)</param>
        /// <param name="I">с.в. интервала появления заявок (ч)</param>
        public void GenerateRequestFlow(double st, Stochastic Q, Stochastic L, Stochastic I)
        {
            reqFlow = new RequestFlow(st, Q, L, I);
        }

    }

    /// <summary>
    /// Грузовладелец (класс)
    /// </summary>
    public class FreightOwner : MarketSubject
    {
        /// <summary>
        /// Номинальная грузоподъемность транспортного пакета, т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PacketCapacity;
        /// <summary>
        /// Коэффициент, учитывающий оборачиваемость транспортировочных емкостей.
        /// Если емкость не возвращается грузоотправителю, то К = 1;
        /// если договор доставки предусматривает возврат грузовой емкости, то К является величиной,
        /// обратной среднему количеству оборотов грузовой емкости до списания;
        /// если стоимость транспортировочной емкости включена в стоимость товара для грузополучателя, то К = 0.
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double TurnoverCoefficient;
        /// <summary>
        /// Стоимость упаковочных материалов, $/пакет;
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PackageMaterialsCosts;
        /// <summary>
        /// Стоимость транспортировочной емкости, $/пакет;
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PackageCosts;
        /// <summary>
        /// Cтоимость 1 ч работы работника, формирующего транспортные пакеты, $/ч
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double PackWrokerCosts;
        /// <summary>
        /// Время на формирование одного транспортного пакета (для грузовладельца), ч/пакет
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public Stochastic PackFormTime;
        /// <summary>
        /// Тариф подрядчика по осуществлению ПРР, $/т
        /// количество операций по погрузке-разгрузке партии товара
        /// NLU=atn+2 (в таможенных пунктах предполагается прямая перевалка)
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double LUTariff;
        /// <summary>
        /// Cтоимость 1 т товара, $/т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double CargoCosts;

        /// <summary>
        /// Исходящий поток заявок
        /// </summary>
        RequestFlow demand;

        /// <summary>
        /// Конструктор класса по умолчанию
        /// </summary>
        public FreightOwner()
        {
            Name = "Unknown Freight Owner";
            //Loc = new Location();

            CargoCosts = 1000;
            demand = new RequestFlow();
        }

        public override RequestFlow GetRequestFlow() { return demand; }

        /// <summary>
        /// Генерирует спрос грузовладельца с заданными характеристиками потока
        /// </summary>
        /// <param name="st">Время моделирования, ч</param>
        /// <param name="Q">с.в. партии груза (т)</param>
        /// <param name="L">с.в. расстояния доставки (км)</param>
        /// <param name="I">с.в. интервала появления заявок (ч)</param>
        public void GenerateDemand(double st, Stochastic Q, Stochastic L, Stochastic I)
        {
            demand = new RequestFlow(st, Q, L, I);
        }

    }

    /// <summary>
    /// Грузовой терминал (класс)
    /// </summary>
    public class FreightTerminal : MarketSubject
    {
        public Warehouse wHouse;
        RequestFlow reqFlow;

        /// <summary>
        /// Время промежуточного хранения партии груза на складе грузового терминала, ч
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public Stochastic StorageTime;
        /// <summary>
        /// Доля стоимости приобретаемых услуг и товаров в себестоимости услуг терминала
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double ExpensesPaid;
        /// <summary>
        /// Общий (по совокупности услуг) тариф на услуги терминала, $/т
        /// (поле присутствует также в классе NumericParameters)
        /// </summary>
        public double Tariff;

        public FreightTerminal()
        {
            Name = "Unknown Freight Terminal";
            //Loc = new Location();
            wHouse = new Warehouse();
            reqFlow = new RequestFlow();
        }

        public FreightTerminal(Warehouse w)
        {
            Name = "Unknown Freight Terminal";
            //Loc = new Location();
            wHouse = new Warehouse(); wHouse = w;
            reqFlow = new RequestFlow();
        }

        public override RequestFlow GetRequestFlow() { return reqFlow; }

    }

}
