using Sirenix.OdinInspector;

namespace JengaGame
{
    public class StackData
    {
        [ShowInInspector]public string Id { get; private set; }
        [ShowInInspector]public string Subject { get; private set; }
        [ShowInInspector]public string Grade { get; private set; }
        [ShowInInspector]public int Mastery { get; private set; }
        [ShowInInspector]public string DomainId { get; private set; }
        [ShowInInspector]public string Domain { get; private set; }
        [ShowInInspector]public string Cluster { get; private set; }
        [ShowInInspector]public string StandardId { get; private set; }
        [ShowInInspector]public string StandardDescription { get; private set; }

        public StackData(string id, string subject, string grade, int mastery, string domainId, string domain,
            string cluster,
            string standardId, string standardDescription)
        {
            Id = id;
            Subject = subject;
            Grade = grade;
            Mastery = mastery;
            DomainId = domainId;
            Domain = domain;
            Cluster = cluster;
            StandardId = standardId;
            StandardDescription = standardDescription;
        }
    }
}