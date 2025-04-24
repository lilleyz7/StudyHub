using Microsoft.AspNetCore.Identity;

namespace StudyHub.Models
{
    public class CustomUser: IdentityUser
    {
        public List<StudyRoom> rooms = new List<StudyRoom>();
    }
}
