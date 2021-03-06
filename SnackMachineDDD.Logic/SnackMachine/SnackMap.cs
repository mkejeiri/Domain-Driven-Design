﻿using FluentNHibernate.Mapping;

namespace SnackMachineDDD.logic.SnackMachine
{
   public class SnackMap: ClassMap<Snack>
    {
        public SnackMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
