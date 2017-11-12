﻿namespace IntegrationTests.Scenarios
{
    using System;
    using System.Data.Entity.Validation;
    using System.Transactions;

    public abstract class TransactionalScenario
    {
        public void RollBack(Action action)
        {
            using (new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    action();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine(eve.Entry.Entity.ToString());
                        Console.WriteLine($"  {ve.PropertyName}: {ve.ErrorMessage}");
                    }
                    throw;
                }
            }
        }
    }
}
