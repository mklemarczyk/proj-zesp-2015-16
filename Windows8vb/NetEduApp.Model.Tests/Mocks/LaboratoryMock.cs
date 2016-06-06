using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightMock;
using NetEduApp.Model.ViewModels;

namespace NetEduApp.Model.Tests.Mocks {
	public class LaboratoryMock : Laboratory {
		private readonly IInvocationContext<Laboratory> context;

		public LaboratoryMock(IInvocationContext<Laboratory> context) {
			this.context = context;
		}

	}
}
