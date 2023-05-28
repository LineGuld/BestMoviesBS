namespace BestMoviesBS.Models
{

    public class DeleteResult
    {
        public DeleteResult(int countersNodesDeleted, int countersRelationshipsDeleted)
        {
            DeletedNodes = countersNodesDeleted;
            DeletedRelationships = countersRelationshipsDeleted;
        }

        public int DeletedNodes { get; set; }
        public int DeletedRelationships { get; set; }
    }
}