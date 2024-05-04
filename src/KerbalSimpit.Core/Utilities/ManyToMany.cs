using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Utilities
{
    public class ManyToMany<T1, T2>
    {
        private Dictionary<T1, List<T2>> _t1Tot2;
        private Dictionary<T2, List<T1>> _t2Tot1;

        public ManyToMany()
        {
            _t1Tot2 = new Dictionary<T1, List<T2>>();
            _t2Tot1 = new Dictionary<T2, List<T1>>();
        }

        public void Add(T1 item1, T2 item2)
        {
            this.GetT2List(item1, out List<T2> item2s);
            this.GetT1List(item2, out List<T1> item1s);

            item1s.Add(item1);
            item2s.Add(item2);
        }

        public void Remove(T1 item1)
        {
            this.GetT2List(item1, out List<T2> item2s);
            foreach(T2 item2 in item2s)
            {
                this.GetT1List(item2, out List<T1> item1s);
                item1s.Remove(item1);
            }

            item2s.Clear();
        }

        public void Remove(T2 item2)
        {
            this.GetT1List(item2, out List<T1> item1s);
            foreach (T1 item1 in item1s)
            {
                this.GetT2List(item1, out List<T2> item2s);
                item2s.Remove(item2);
            }

            item1s.Clear();
        }

        public void Clear()
        {
            _t1Tot2.Clear();
            _t2Tot1.Clear();
        }

        private void GetT2List(T1 item1, out List<T2> item2s)
        {
            if (_t1Tot2.TryGetValue(item1, out item2s) == false)
            {
                item2s = new List<T2>();
                _t1Tot2.Add(item1, item2s);
            }
        }

        private void GetT1List(T2 item2, out List<T1> item1s)
        {
            if (_t2Tot1.TryGetValue(item2, out item1s) == false)
            {
                item1s = new List<T1>();
                _t2Tot1.Add(item2, item1s);
            }
        }
    }
}
