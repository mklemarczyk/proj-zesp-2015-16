using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightMock;
using NetEduApp.Model.ViewModels;

namespace NetEduApp.Model.Tests.Mocks {
	public class DeviceViewModelMock : DeviceViewModel {
		private readonly IInvocationContext<DeviceViewModel> context;

		public DeviceViewModelMock(IInvocationContext<DeviceViewModel> context, IInvocationContext<Laboratory> labContext)
			: base(new LaboratoryMock(labContext)) {
			this.context = context;
		}

		public override string ImagePath {
			get {
				return context.Invoke(f => f.ImagePath);
			}
		}

		protected override string GetNamePattern( ) {
			if (context != null) {
				return context.Invoke(f => string.Empty);
			}
			return string.Empty;
		}
	}
}
