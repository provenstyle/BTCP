namespace BibleTraining.Api.EmailType
{
    using Improving.MediatR;

    public class EmailTypeData : Resource<int>
    {
        public string Name { get; set; }

    }
}