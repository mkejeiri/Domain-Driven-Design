﻿using SnackMachineDDD.logic.Utils;
namespace SnackMachineDDD.UI{
    public partial class App
    {
        public App()
        {
            Initer.Init();
            //using (ISession session = SessionFactory.OpenSession())
            //{
            //    long id = 1;
            //    var snackMachine = session.Get<SnackMachine>(id);
            //}
        }
    }
}
