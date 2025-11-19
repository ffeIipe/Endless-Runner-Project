using Enums;
using UnityEngine;

namespace Components
{
    public class TeamComponent
    {
        private readonly TeamType _currentTeamType;
        
        public TeamComponent(TeamType teamType)
        {
            _currentTeamType = teamType;
        }

        public TeamType GetCurrentTeam() => _currentTeamType;
    }
}