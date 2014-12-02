namespace SDammann.WebApi.Versioning.Tests.Discovery {
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestSupport.Version1;
    using Versioning.Discovery;

    [TestClass]
    public class TypeNameControllerNameDetectorTests {

        [TestMethod]
        public void DefaultControllerNameDetector_DetectsCatController_AsNameCat() {
            // given
            IControllerNameDetector detector = new DefaultControllerNameDetector();
            Type catControllerType = typeof (CatController);

            // when
            string controllerName = detector.GetControllerName(catControllerType);

            // assert
            Assert.AreEqual("Cat", controllerName, true);
        }


        [TestMethod]
        public void DefaultControllerNameDetector_DetectsCatCtrl_AsUnknown()
        {
            // given
            IControllerNameDetector detector = new DefaultControllerNameDetector();
            Type catControllerType = typeof(CatCtrl);

            // when
            string controllerName = detector.GetControllerName(catControllerType);

            // assert
            Assert.IsNull(controllerName);
        }

        [TestMethod]
        public void DefaultControllerNameDetector_DetectsController_AsUnknown()
        {
            // given
            IControllerNameDetector detector = new DefaultControllerNameDetector();
            Type catControllerType = typeof(Controller);

            // when
            string controllerName = detector.GetControllerName(catControllerType);

            // assert
            Assert.IsNull(controllerName);
        }

    }
}