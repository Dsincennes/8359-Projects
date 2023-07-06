using Lab4.Models;

namespace Lab4.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SportsDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Fans.Any())
            {
                return;   // DB has been seeded
            }

            var fans = new Fan[]
            {
            new Fan{FirstName="Carson",LastName="Alexander",BirthDate=DateTime.Parse("1995-01-09")},
            new Fan{FirstName="Meredith",LastName="Alonso",BirthDate=DateTime.Parse("1992-09-05")},
            new Fan{FirstName="Arturo",LastName="Anand",BirthDate=DateTime.Parse("1993-10-09")},
            new Fan{FirstName="Gytis",LastName="Barzdukas",BirthDate=DateTime.Parse("1992-01-09")},
            };
            foreach (Fan s in fans)
            {
                context.Fans.Add(s);
            }
            context.SaveChanges();

            var sportClubs = new SportClub[]
            {
            new SportClub{ID="A1",Title="Alpha",Fee=300},
            new SportClub{ID="B1",Title="Beta",Fee=130},
            new SportClub{ID="O1",Title="Omega",Fee=390},
            };
            foreach (SportClub c in sportClubs)
            {
                context.SportClubs.Add(c);
            }
            context.SaveChanges();

            var subscriptions = new Subscription[]
            {
            new Subscription{FanID=1,SportClubID="A1"},
            new Subscription{FanID=1,SportClubID="B1"},
            new Subscription{FanID=1,SportClubID="O1"},
            new Subscription{FanID=2,SportClubID="A1"},
            new Subscription{FanID=2,SportClubID="B1"},
            new Subscription{FanID=3,SportClubID="A1"},
            };
            foreach (var subscription in subscriptions)
            {
                context.Subscriptions.Add(subscription);
            }
            context.SaveChanges();

        }
    }

}
