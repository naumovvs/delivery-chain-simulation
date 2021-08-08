using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StochasticValues;

namespace TransportMarket
{
    /// <summary>
    /// Логистическая цепочка, позволяющая реализовать заявку
    /// </summary>
    public class LogisticsChain
    {
        /// <summary>
        /// численные параметры
        /// </summary>
        public static NumericParameters np = new NumericParameters();
        /// <summary>
        /// заявка
        /// </summary>
        Request req;

        public List<string> types;      // доступные варианты схем "ILI", "II", ...
        public List<string> variants;   // список альтернативных вариантов структуры ЛЦ

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="r">Заявка, для которой реализуетя логистическая цепочка</param>
        public LogisticsChain(Request r)
        {
            req = r;

            DefineTypes();
            DefineVariants();
        }

        /// <summary>
        /// Конструктор класса с дополнительными параметрами заявки
        /// </summary>
        /// <param name="r">Заявка, для которой реализуетя логистическая цепочка</param>
        /// <param name="atn">доступное количество грузовых терминалов</param>
        /// <param name="acn">количество таможенных переходов</param>
        /// <param name="it">доступные виды подвозочного транспорта</param>
        /// <param name="lt">доступные виды магистрального транспорта</param>
        public LogisticsChain(Request r, int atn, int acn, InlandTransport it, LinehaulTransport lt)
        {
            // ???? нужна ли? ведь доп. хар-ки являются х-ками заявки, а не потока

            req = r;
            r.SetAdditionalParams(atn, acn, it, lt);

            DefineTypes();
            DefineVariants();
        }

        /// <summary>
        /// Определение списка доступных вариантов схем
        /// </summary>
        public void DefineTypes()
        {
            // "I" - подвозочный транспорт
            // "L" - магистральный транспорт

            types = new List<string>(); // очистка текущего списка
            if (req.TNum == 0 && req.CNum == 0) types.Add("I");
            if (req.TNum == 1 && req.CNum == 0) types.Add("II");
            if (req.TNum == 2 && req.CNum == 0) types.Add("ILI");

            if (req.TNum == 0 && req.CNum == 1)
            { types.Add("I"); types.Add("II"); }
            if (req.TNum == 1 && req.CNum == 1)
            { types.Add("I"); types.Add("II"); types.Add("III"); }
            if (req.TNum == 2 && req.CNum == 1)
            { types.Add("ILI"); types.Add("IILI"); types.Add("ILII"); }

            if (req.TNum == 0 && req.CNum == 2)
            { types.Add("I"); types.Add("II"); types.Add("III"); }
            if (req.TNum == 1 && req.CNum == 2)
            { types.Add("I"); types.Add("II"); types.Add("III"); types.Add("IIII"); }
            if (req.TNum == 2 && req.CNum == 2)
            {
                types.Add("ILI"); types.Add("IILI"); types.Add("ILII");
                types.Add("IIILI"); types.Add("ILIII"); types.Add("IILII");
            }

        }

        /// <summary>
        /// Расчет количества вариантов для заданного типа схемы
        /// </summary>
        /// <param name="ts">тип схемы</param>
        /// <returns>Количество вариантов ЛЦ</returns>
        private int VarNumInType(string ts)
        {
            int vn = 1;
            int itn = 0, ltn = 0;

            if (req.availableIT.Road) itn++;
            if (req.availableIT.Railway) itn++;
            if (req.availableIT.Riverine) itn++;

            if (req.availableLT.Road) ltn++;
            if (req.availableLT.Railway) ltn++;
            if (req.availableLT.Riverine) ltn++;
            if (req.availableLT.Marine) ltn++;
            if (req.availableLT.Air) ltn++;

            for (int i = 0; i < ts.Length; i++)
            {
                if (ts[i] == 'I' || ts[i] == 'i')
                    vn *= itn;
                else
                    vn *= ltn;
            }

            return vn;
        }

        /// <summary>
        /// Расчет вариантов структуры ЛЦ
        /// </summary>
        public void DefineVariants()
        {
            variants = new List<string>();
            //
            List<string> cv = new List<string>();
            List<string> CreatedVars = new List<string>();

            foreach (string s in types)
            {
                CreatedVars = new List<string>(); // список вариантов для текущего types: содержит как полные, так и неполные варианты
                for (int i = 0; i < s.Length; i++) // перебор элементов types
                {
                    cv = new List<string>(CreatedVars); // список промежуточных вариантов
                    if (cv.Count > 0) // коллекция уже содержит промежуточные варианты
                    {
                        foreach (string c in CreatedVars) // рассматриваем каждый созданный ранее вариант
                        {
                            if (s[i] == 'I' || s[i] == 'i')  // рассматриваемый транспорт - подвозочный
                            {
                                if (req.availableIT.Road) cv.Add(c + "1");
                                if (req.availableIT.Railway) cv.Add(c + "2");
                                if (req.availableIT.Riverine) cv.Add(c + "3");
                            }
                            else // рассматриваемый транспорт - магистральный
                            {
                                if (req.availableLT.Road) cv.Add(c + "4");
                                if (req.availableLT.Railway) cv.Add(c + "5");
                                if (req.availableLT.Riverine) cv.Add(c + "6");
                                if (req.availableLT.Marine) cv.Add(c + "7");
                                if (req.availableLT.Air) cv.Add(c + "8");
                            }
                        } // end foreach
                    }
                    else // в коллекции еще нет вариантов
                    {
                        if (s[i] == 'I' || s[i] == 'i')  // рассматриваемый транспорт - подвозочный
                        {
                            if (req.availableIT.Road) cv.Add("1");
                            if (req.availableIT.Railway) cv.Add("2");
                            if (req.availableIT.Riverine) cv.Add("3");
                        }
                        else // рассматриваемый транспорт - магистральный
                        {
                            if (req.availableLT.Road) cv.Add("4");
                            if (req.availableLT.Railway) cv.Add("5");
                            if (req.availableLT.Riverine) cv.Add("6");
                            if (req.availableLT.Marine) cv.Add("7");
                            if (req.availableLT.Air) cv.Add("8");
                        }
                        // добавить в коллекцию промежуточных вариантов
                    } // end if
                    CreatedVars.AddRange(cv);
                } // next i

                for (int i = cv.Count - 1; i > cv.Count - VarNumInType(s) - 1; i--)
                    variants.Add(cv[i]); // добавить окончательные варианты рассмотренного типа в variants

            } // end foreach of types

        }

        /// <summary>
        /// Расчет суммарных затрат для варианта ЛЦ
        /// </summary>
        /// <param name="var">вариант ЛЦ</param>
        /// <returns>суммарные затраты по ЛЦ</returns>
        public double CountVarCosts(string var)
        {
            return ForwarderExpenses() + OwnerExpenses(var) +
                TerminalExpenses() + CarriersExpenses(var);
        }

        /// <summary>
        /// Определение оптимального варианта ЛЦ
        /// </summary>
        /// <returns>опитимальный вариант ЛЦ</returns>
        public string GetOptimalChain()
        {
            string opt = "";
            double[] costs;
            double minCosts;

            if (variants.Count > 0)
            {
                costs = new double[variants.Count];
                minCosts = CountVarCosts(variants[0]);
                opt = variants[0];
                if (variants.Count > 1)
                    for (int i = 1; i < variants.Count; i++)
                    {
                        costs[i] = CountVarCosts(variants[i]);
                        if (costs[i] < minCosts)
                        {
                            minCosts = costs[i];
                            opt = variants[i];
                        }
                    }
            }
            return opt;
        }

        /// <summary>
        /// Затраты экспедитора
        /// </summary>
        /// <param name="var">вариант структуры ЛЦ</param>
        /// <returns>суммарные затраты экспедитора</returns>
        public double ForwarderExpenses()
        {
            double a0, aI;
            double expenses = 0;

            // for risks analisys:
            double opTime = (new Stochastic(1, np.FFReqOpTime, np.FFReqOpTime / 3, 0)).GetValue();
            // opTime = np.FFReqOpTime;

            a0 = np.FFDispatcher1HourCosts * (1 - np.FFDispatchersNumber) * opTime / (100 + np.VATRate) *
                    (100 + (np.FFProfitability + 1) * np.VATRate -
                    (1 + 0.01 * np.VATRate - np.FFProfitability) * np.ProfitTaxRate);
            aI = np.FFDispatcher1HourCosts * np.FFDispatchersNumber +
                    np.VATRate * (np.FFDispatcher1HourCosts * np.FFDispatchersNumber * np.FFProfitability -
                    np.FFPaid1HourCosts) / (100 + np.VATRate) +
                    np.ProfitTaxRate * (0.02 * np.VATRate * np.FFPaid1HourCosts -
                    np.FFDispatchersNumber * np.FFDispatcher1HourCosts *
                    (1 + 0.01 * np.VATRate - np.FFProfitability)) / (100 + np.VATRate);

            expenses = a0 + aI * req.I;

            return expenses;
        }

        /// <summary>
        /// Затраты грузовладельца
        /// </summary>
        /// <param name="var">вариант структуры ЛЦ</param>
        /// <returns>суммарные затраты грузовладельца</returns>
        public double OwnerExpenses(string var)
        {
            double a0, aQ2, aQL, aQ, aI;
            double expenses = 0;

            bool containsLinehaul = var.Contains('4') || var.Contains('5') || var.Contains('6') || var.Contains('7') || var.Contains('8');
            double delta = (containsLinehaul) ? (1 - np.LinehaulPriority) / (var.Length - 1) : 1 / var.Length;

            // for risks analisys:
            double opTime = (new Stochastic(1, np.FFReqOpTime, np.FFReqOpTime / 3, 0)).GetValue();
            // opTime = np.FFReqOpTime;

            a0 = np.FFDispatcher1HourCosts * (1 + np.FFProfitability) * opTime * (1 - np.FFDispatchersNumber);

            aQ2 = np.FTLoadTime.GetValue() + np.FTUnloadTime.GetValue();
            // расчет циклом по количеству перегрузок, чтобы генерировать каждый раз новые значения времени на погрузку и разгрузку
            for (int i = 0; i < var.Length - 1; i++)
                aQ2 += np.FTLoadTime.GetValue() + np.FTUnloadTime.GetValue();
            aQ2 *= np.CargoCosts * np.DepositeRate / (24 * 100 * 365);

            aQ = 0;
            // расчет циклом по количеству таможенных переходов
            for (int i = 0; i < req.CNum; i++)
                aQ += np.CustomsDetention.GetValue();
            aQ *= np.CargoCosts * np.DepositeRate / (24 * 100 * 365);
            int NLU = 2 + var.Length - req.TNum - 1; // количество перегрузок с использованием подрядчиков
            aQ += NLU * np.LUTariff + req.TNum * np.FTTariff;
            aQ += 0.01 * np.CargoCosts * req.CNum * (np.DutyRate + np.ImportRate);
            aQ += (np.FOPackFormTime.GetValue() * np.FOPackWrokerCosts +
                np.PackageMaterialsCosts + np.PackageCosts * np.TurnoverCoefficient) / np.PacketCapacity;

            aQL = 0;
            for (int i = 0; i < var.Length; i++)
            {
                switch (var[i])
                {
                    case '1':
                        aQL += (np.k[0] * delta / np.k[0]) * (np.ITroad.Tariff +
                            np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.ITroad.Velocity.GetValue()));
                        break;
                    case '2':
                        aQL += (np.k[1] * delta / np.k[0]) * (np.ITrailway.Tariff +
                            np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.ITrailway.Velocity.GetValue()));
                        break;
                    case '3':
                        aQL += (np.k[2] * delta / np.k[0]) * (np.ITriverine.Tariff +
                            np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.ITriverine.Velocity.GetValue()));
                        break;
                    case '4':
                        aQL += (np.k[3] * np.LinehaulPriority / np.k[0]) * (np.LTroad.Tariff +
                            np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.LTroad.Velocity.GetValue()));
                        break;
                    case '5':
                        aQL += (np.k[4] * np.LinehaulPriority / np.k[0]) * (np.LTrailway.Tariff +
                            np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.LTrailway.Velocity.GetValue()));
                        break;
                    case '6':
                        aQL += (np.k[5] * np.LinehaulPriority / np.k[0]) * (np.LTriverine.Tariff +
                           np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.LTriverine.Velocity.GetValue()));
                        break;
                    case '7':
                        aQL += (np.k[6] * np.LinehaulPriority / np.k[0]) * (np.LTmarine.Tariff +
                           np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.LTmarine.Velocity.GetValue()));
                        break;
                    case '8':
                        aQL += (np.k[7] * np.LinehaulPriority / np.k[0]) * (np.LTair.Tariff +
                           np.CargoCosts * np.DepositeRate / (356 * 24 * 100 * np.LTair.Velocity.GetValue()));
                        break;
                }
            }

            aI = np.FFDispatchersNumber * np.FFDispatcher1HourCosts * (1 + np.FFProfitability);

            expenses = a0 + aQ2 * req.Q * req.Q + aQ * req.Q + aQL * req.Q * req.L + aI * req.I;

            return expenses;
        }

        /// <summary>
        /// Затраты грузовых терминалов
        /// </summary>
        /// <param name="var">вариант структуры ЛЦ</param>
        /// <returns>суммарные затраты грузовых терминалов</returns>
        public double TerminalExpenses()
        {
            double aQ;
            double expenses = 0;

            for (int i = 0; i < req.TNum; i++)
            {
                aQ = (np.FTReloadCosts * (np.FTLoadTime.GetValue() + np.FTUnloadTime.GetValue()) +
                    np.FTPackWorkerCosts * np.FTPackFormTime.GetValue() +
                    np.FTStorageCosts * np.FTStorageTime.GetValue()) *
                    (100 + np.VATRate - np.FTExpensesPaid * np.VATRate + np.FTExpensesPaid * np.ProfitTaxRate -
                    0.01 * np.FTExpensesPaid * np.VATRate * np.ProfitTaxRate) / (100 + np.VATRate) +
                    np.FTTariff * (np.VATRate + np.ProfitTaxRate) / (100 + np.VATRate);
                expenses += aQ * req.Q;
            }

            return expenses;
        }

        /// <summary>
        /// Затраты перевозчиков
        /// </summary>
        /// <param name="var">вариант структуры ЛЦ</param>
        /// <returns>суммарные затраты перевозчиков</returns>
        public double CarriersExpenses(string var)
        {
            double a0 = 0, aQL = 0, aQ = 0, aL = 0;
            double expenses = 0;

            bool containsLinehaul = var.Contains('4') || var.Contains('5') || var.Contains('6') || var.Contains('7') || var.Contains('8');
            //Console.WriteLine(containsLinehaul);
            double delta = (containsLinehaul) ? (1 - np.LinehaulPriority) / (var.Length - 1) : 1 / (double)var.Length;
            //Console.WriteLine(delta);
            int cnAdded = 0;    // количество учтенных таможенных пунктов
            // по алгоритму таможенные пункты проходят первые 2 вида транспорта, что не всегда корректно
            for (int i = 0; i < var.Length; i++)
            {
                switch (var[i])
                {
                    case '1':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.ITroad.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.ITroad.Costs1Hour *
                            (np.ITroad.LoadDuration.GetValue() + np.ITroad.UnloadDuration.GetValue());
                        aL += (np.k[0] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.ITroad.Costs1Km + np.ITroad.Costs1Hour / np.ITroad.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.ITroad.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[0] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.ITroad.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.ITroad.Tariff);
                        break;
                    case '2':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.ITrailway.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.ITrailway.Costs1Hour *
                            (np.ITrailway.LoadDuration.GetValue() + np.ITrailway.UnloadDuration.GetValue());
                        aL += (np.k[1] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.ITrailway.Costs1Km + np.ITrailway.Costs1Hour / np.ITrailway.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.ITrailway.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[1] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.ITrailway.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.ITrailway.Tariff);
                        break;
                    case '3':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.ITriverine.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.ITriverine.Costs1Hour *
                            (np.ITriverine.LoadDuration.GetValue() + np.ITriverine.UnloadDuration.GetValue());
                        aL += (np.k[2] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.ITriverine.Costs1Km + np.ITriverine.Costs1Hour / np.ITriverine.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.ITriverine.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[2] * delta / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.ITriverine.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.ITriverine.Tariff);
                        break;
                    case '4':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.LTroad.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.LTroad.Costs1Hour *
                            (np.LTroad.LoadDuration.GetValue() + np.LTroad.UnloadDuration.GetValue());
                        aL += (np.k[3] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.LTroad.Costs1Km + np.LTroad.Costs1Hour / np.LTroad.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.LTroad.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[3] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.LTroad.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.LTroad.Tariff);
                        break;
                    case '5':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.LTrailway.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.LTrailway.Costs1Hour *
                            (np.LTrailway.LoadDuration.GetValue() + np.LTrailway.UnloadDuration.GetValue());
                        aL += (np.k[4] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.LTrailway.Costs1Km + np.LTrailway.Costs1Hour / np.LTrailway.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.LTrailway.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[4] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.LTrailway.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.LTrailway.Tariff);
                        break;
                    case '6':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.LTriverine.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.LTriverine.Costs1Hour *
                            (np.LTriverine.LoadDuration.GetValue() + np.LTriverine.UnloadDuration.GetValue());
                        aL += (np.k[5] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.LTriverine.Costs1Km + np.LTriverine.Costs1Hour / np.LTriverine.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.LTriverine.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[5] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.LTriverine.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.LTriverine.Tariff);
                        break;
                    case '7':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.LTmarine.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.LTmarine.Costs1Hour *
                            (np.LTmarine.LoadDuration.GetValue() + np.LTmarine.UnloadDuration.GetValue());
                        aL += (np.k[6] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.LTmarine.Costs1Km + np.LTmarine.Costs1Hour / np.LTmarine.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.LTmarine.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[6] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.LTmarine.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.LTmarine.Tariff);
                        break;
                    case '8':
                        if (req.CNum > 0 && cnAdded < req.CNum)
                        {
                            a0 += (1 - 0.01 * np.ProfitTaxRate) * np.LTair.Costs1Hour * np.CustomsDetention.GetValue();
                            cnAdded++;
                        }
                        aQ += (1 - 0.01 * np.ProfitTaxRate) * np.LTair.Costs1Hour *
                            (np.LTair.LoadDuration.GetValue() + np.LTair.UnloadDuration.GetValue());
                        aL += (np.k[7] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * (np.LTair.Costs1Km + np.LTair.Costs1Hour / np.LTair.Velocity.GetValue()) +
                            (0.02 * np.ProfitTaxRate - 1) * np.VATRate * np.LTair.Costs1Km / (100 + np.VATRate));
                        aQL += (np.k[7] * np.LinehaulPriority / np.k[0]) *
                            ((1 - 0.01 * np.ProfitTaxRate) * np.VATRate * np.LTair.Tariff / (100 + np.VATRate) +
                            0.01 * np.ProfitTaxRate * np.LTair.Tariff);
                        break;
                }
            }

            expenses = a0 + aQ * req.Q + aQL * req.Q * req.L + aL * req.L;
            return expenses;
        }

        /// <summary>
        /// Прибыль экспедитора
        /// </summary>
        /// <param name="tarifType">тип тарифа: true - на основании нормы рентабельности, false - постоянный на заявку</param>
        /// <param name="Tariff"> значение нормы рентабельности || величина постоянного тарифа</param>
        /// <returns></returns>
        public double ForwarderProfit(bool tarifType, double Tariff)
        {
            // доход и затраты
            double income = 0, costs = 0;
            // ПДВ, чистая прибыль, налог на прибыль
            double vat = 0, pp = 0, pt = 0;

            if (tarifType)
            {
                income = (np.FFDispatcher1HourCosts * np.FFDispatchersNumber * (req.I - np.FFReqOpTime) +
                    np.FFDispatcher1HourCosts * np.FFReqOpTime) * (1 + np.FFProfitability);
                costs = ForwarderExpenses();
            }
            else
            {
                income = Tariff;
                costs = np.FFDispatcher1HourCosts * np.FFDispatchersNumber * (req.I - np.FFReqOpTime) +
                    np.FFDispatcher1HourCosts * np.FFReqOpTime;
                vat = np.VATRate * (income - np.FFPaid1HourCosts * req.I) / (100 + np.VATRate);
                pp = income - costs - vat + np.VATRate * np.FFPaid1HourCosts * req.I / (100 + np.VATRate);
                if (pp > 0) pt = 0.01 * pp * np.ProfitTaxRate;
                costs += vat + pt;
            }
            return (income - costs);
        }

    }
}
