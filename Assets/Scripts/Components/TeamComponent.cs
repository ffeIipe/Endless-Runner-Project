using Enums;
using UnityEngine;

namespace Components
{
    public class TeamComponent
    {
        private readonly Team _currentTeam;
        
        public TeamComponent(Team team)
        {
            _currentTeam = team;
        }

        public Team GetCurrentTeam() => _currentTeam;
    }
}