using System;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverRobotAgent
    {
        private Func<PlanningResult> _plan;
        private Action _perceive;
        private Action _act;
        private Action _react;
        private Action _clearState;

        public Action Perceive
        {
            get => _perceive;
            set => _perceive = value ?? (() => {});
        }

        public Func<PlanningResult> Plan
        {
            get => _plan;
            set => _plan = value?? (() => PlanningResult.NoPlan);
        }

        public Action Act
        {
            get => _act;
            set => _act = value ?? (() => { });
        }

        public Action React
        {
            get => _react;
            set => _react = value ?? (() => { });
        }

        public Action ClearState
        {
            get => _clearState;
            set => _clearState = value ?? (() => { });
        }
    }
}