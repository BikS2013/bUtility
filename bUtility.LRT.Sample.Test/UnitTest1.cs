using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace bUtility.LRT.Sample.Test
{
    [TestClass]
    public class UnitTest1
    {

        void testAction(IPolicyAction action)
        {
            Assert.IsNotNull(action);

            Assert.IsTrue(action.IsCompleted());

            Assert.IsFalse(action.IsReversed());
        }

        void testOperation( Sample.Operation op )
        {
            var res = op.Execute();
            Assert.IsTrue(res);
            Assert.IsNotNull(op.Actions);
            Assert.AreEqual(op.Actions.Count, 4);

            testAction( op.Actions.FirstOrDefault(a => a.Order == 0));
            testAction( op.Actions.FirstOrDefault(a => a.Order == 1));
            testAction( op.Actions.FirstOrDefault(a => a.Order == 2));
            testAction( op.Actions.FirstOrDefault(a => a.Order == 3));
        }


        [TestMethod]
        public void TestMethod1()
        {
            var id = Guid.NewGuid();
            var bid = Guid.NewGuid();
            var sys = new SystemNoErrors();

            var store = new InMemoryStore.Store(id, bid);
            var op = new Sample.Operation(store, sys, new Request { });
            testOperation(op);



            var info = store.GetInfo();
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.Actions);
            Assert.AreEqual(info.Actions.Count, 4);

            Assert.AreEqual(info.Status, InMemoryStore.Store.opStatus.completed);
            var ainfo0 = info.Actions.FirstOrDefault(a => a.Order == 0);
            var ainfo1 = info.Actions.FirstOrDefault(a => a.Order == 1);
            var ainfo2 = info.Actions.FirstOrDefault(a => a.Order == 2);
            var ainfo3 = info.Actions.FirstOrDefault(a => a.Order == 3);
            Assert.IsTrue( ainfo0.IsLast && ainfo1.IsLast && ainfo2.IsLast && ainfo3.IsLast);
        }
    }
}
