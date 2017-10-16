namespace BibleTraining.Entities
{
    using System;
    using Concurrency;
    using Improving.Highway.Data.Scope;

    public class Entity : IEntity, IRowVersioned
    {
        public int      Id         { get; set; }
        public byte[]   RowVersion { get; set; }
        public DateTime Created    { get; set; }
        public string   CreatedBy  { get; set; }
        public DateTime Modified   { get; set; }
        public string   ModifiedBy { get; set; }
    }
}