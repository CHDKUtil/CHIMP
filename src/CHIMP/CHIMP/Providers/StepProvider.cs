using Chimp.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers
{
    sealed class StepProvider : IStepProvider
    {
        #region Fields

        private StepsData StepsData { get; }

        #endregion

        #region Constructor

        public StepProvider(IOptions<StepsData> options)
        {
            StepsData = options.Value;

            _steps = new Lazy<Dictionary<string, StepData>>(GetSteps);
        }

        #endregion

        #region IStepProvider Members

        IEnumerable<string> IStepProvider.GetSteps()
        {
            return StepsData.Steps.Select(s => s.Name);
        }

        public bool IsSkip(string name)
        {
            return Steps[name].Skip;
        }

        public bool IsHidden(string name)
        {
            return Steps[name].Hidden;
        }

        #endregion

        #region Steps

        private readonly Lazy<Dictionary<string, StepData>> _steps;

        private Dictionary<string, StepData> Steps => _steps.Value;

        private Dictionary<string, StepData> GetSteps()
        {
            return StepsData.Steps.ToDictionary(s => s.Name, s => s);
        }

        #endregion
    }
}
