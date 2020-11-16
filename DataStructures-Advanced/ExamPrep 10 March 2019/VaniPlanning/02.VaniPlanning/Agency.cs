namespace _02.VaniPlanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Agency : IAgency
    {
        private Dictionary<string, Invoice> bySerial = new Dictionary<string, Invoice>();

        public void Create(Invoice invoice)
        {
            if (this.bySerial.ContainsKey(invoice.SerialNumber))
            {
                throw new ArgumentException();
            }

            this.bySerial.Add(invoice.SerialNumber, invoice);
        }

        public void ThrowInvoice(string number)
        {
            if (!this.bySerial.ContainsKey(number))
            {
                throw new ArgumentException();
            }

            this.bySerial.Remove(number);
        }

        public void ThrowPayed()
        {
            var toRemove = this.bySerial.Values.Where(x => x.Subtotal == 0).ToList();

            foreach (var item in toRemove)
            {
                this.ThrowInvoice(item.SerialNumber);
            }
        }

        public int Count()
        {
            return this.bySerial.Count;
        }

        public bool Contains(string number)
        {
            return this.bySerial.ContainsKey(number);
        }

        public void PayInvoice(DateTime due)
        {
            var forPay = this.bySerial.Where(x => x.Value.DueDate.Date == due.Date);

            if (forPay.Count() == 0)
            {
                throw new ArgumentException();
            }

            foreach (var item in forPay)
            {
                item.Value.Subtotal = 0;
            }
        }

        public IEnumerable<Invoice> GetAllInvoiceInPeriod(DateTime start, DateTime end)
        {
            var result = this.bySerial.Values.Where(x => x.IssueDate.Date >= start.Date && x.IssueDate.Date <= end.Date)
                .OrderBy(x => x.IssueDate)
                .ThenBy(x => x.DueDate);

            if (result.Count() == 0)
            {
                return Enumerable.Empty<Invoice>();
            }

            return result;
        }

        public IEnumerable<Invoice> SearchBySerialNumber(string serialNumber)
        {
            var result = this.bySerial.Where(x => x.Key.Contains(serialNumber)).Select(x => x.Value).OrderByDescending(x => x.SerialNumber);

            if (result.Count() == 0)
            {
                throw new ArgumentException();
            }

            return result;
        }

        public IEnumerable<Invoice> ThrowInvoiceInPeriod(DateTime start, DateTime end)
        {
            var result = this.bySerial.Values.Where(x => x.DueDate.Date > start.Date && x.DueDate.Date < end.Date).ToList();

            if (result.Count() == 0)
            {
                throw new ArgumentException();
            }

            foreach (var item in result)
            {

                this.ThrowInvoice(item.SerialNumber);
            }

            return result;
        }

        public IEnumerable<Invoice> GetAllFromDepartment(Department department)
        {
            var result = this.bySerial.Values.Where(x => x.Department == department)
                .OrderByDescending(x => x.Subtotal)
                .ThenBy(x => x.IssueDate);

            if (result.Count() == 0)
            {
                return Enumerable.Empty<Invoice>();
            }

            return result;
        }

        public IEnumerable<Invoice> GetAllByCompany(string company)
        {
            var result = this.bySerial.Values.Where(x => x.CompanyName == company)
                .OrderByDescending(x => x.SerialNumber);

            if (result.Count() == 0)
            {
                return Enumerable.Empty<Invoice>();
            }

            return result;
        }

        public void ExtendDeadline(DateTime dueDate, int days)
        {
            var result = this.bySerial.Values.Where(x => x.DueDate.Date == dueDate.Date);

            if (result.Count() == 0)
            {
                throw new ArgumentException();
            }

            foreach (var item in result)
            {
                item.DueDate = item.DueDate.AddDays(days);
            }
        }
    }
}
