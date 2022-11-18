using Domain.MainModule.Entity;
using Domain.MainModule.IRepository;
using Infrastructure.Data.MainModule.Context;

namespace Infrastructure.Data.MainModule.Repository;

public class ChoreRepository : GenericRepository<Chore, int>, IChoreRepository
{
    public ChoreRepository(MainContext mainContext) : base(mainContext)
    {

    }
}