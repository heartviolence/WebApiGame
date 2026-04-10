using Assets.Scripts.Shared.GameDatas;
using Microsoft.Extensions.Logging.Abstractions;
using SampleWebApi.Model.Characters;
using SampleWebApi.Model.Items;
using SampleWebApi.Service;
using SampleWebApi.Service.Characters;
using SampleWebApi.Service.RequestMissions;
using ServerShared.DbContexts;
using ServerShared.Events;
using System.Collections;

namespace ServerTest
{
    public class RequestMissionServiceTest
    {
        RequestMissionService _requestMissionService;

        public static IEnumerable IsMissionSuccessTestCases
        {
            get
            {
                yield return new TestCaseData("00-00-01", new List<GameCharacter> { DefaultGameCharacter.Sora(), DefaultGameCharacter.Sia(), DefaultGameCharacter.Flora() }).Returns(true);
                yield return new TestCaseData("00-00-01", new List<GameCharacter> { DefaultGameCharacter.Sora(), DefaultGameCharacter.Flora() }).Returns(true);
                yield return new TestCaseData("00-00-01", new List<GameCharacter> { DefaultGameCharacter.Sora(), DefaultGameCharacter.Sia() }).Returns(false);
                yield return new TestCaseData("00-00-01", new List<GameCharacter>()).Returns(false);
            }
        }
        public static IEnumerable IsValidCharacterCodeTestCases
        {
            get
            {
                yield return new TestCaseData(new List<string> { CharacterNames.Sora, CharacterNames.Sia, CharacterNames.Flora }).Returns(true);
                yield return new TestCaseData(new List<string> { CharacterNames.Sora, CharacterNames.Flora }).Returns(true);
                yield return new TestCaseData(new List<string> { CharacterNames.Sora, CharacterNames.Sia }).Returns(true);
                yield return new TestCaseData(new List<string>()).Returns(false);
                yield return new TestCaseData(new List<string> { CharacterNames.Sia, CharacterNames.Flora, CharacterNames.Sia }).Returns(false);
                yield return new TestCaseData(new List<string> { CharacterNames.Flora, CharacterNames.Flora }).Returns(false);
                yield return new TestCaseData(new List<string> { CharacterNames.Sora, CharacterNames.Sia, CharacterNames.Nora, CharacterNames.Flora }).Returns(false);
                yield return new TestCaseData(new List<string> { CharacterNames.Sora, CharacterNames.Sia, CharacterNames.Flora, CharacterNames.Flora }).Returns(false);
            }
        }

        [SetUp]
        public void Setup()
        {
            _requestMissionService = new RequestMissionService(new RequestMissionProvider(), new GameCharacterDataProvider(), NullLogger<RequestMissionService>.Instance);
        }

        [TestCaseSource(nameof(IsValidCharacterCodeTestCases))]
        public bool IsValidCharacterCodes(List<string> characterCodes)
        {
            return _requestMissionService.IsValidCharacterCodes(characterCodes);
        }

        [TestCaseSource(nameof(IsMissionSuccessTestCases))]
        public bool IsMissionSuccess(string missionCode, List<GameCharacter> characters)
        {
            return _requestMissionService.IsMissionSuccess(missionCode, characters);
        }

        [TestCase("00-00-01", ExpectedResult = true)]
        [TestCase("00-00-02", ExpectedResult = false)]
        [TestCase("00-00-03", ExpectedResult = false)]
        public bool IsValidMissionCode(string missionCode)
        {
            List<string> testDbData = new()
            {
                "00-00-02"
            };

            return _requestMissionService.IsValidMissionCode(testDbData, missionCode);
        }

        [Test]
        public void ProcessCompleteMission()
        {
            UserAccountDetail userInfo = new()
            {
                Username = "Test",
                Crystal = 0,
            };
            List<GetMissionRewardEvent> createdEvents = _requestMissionService.ProcessCompleteMission(userInfo, "00-00-01");

            Assert.That(userInfo.Crystal == 100);
            Assert.That(createdEvents.Count == 1);
            Assert.That(createdEvents[0].ItemCode == SpeicalItemNames.Crystal);
            Assert.That(createdEvents[0].BeforeItemCount == 0);
            Assert.That(createdEvents[0].AeforeItemCount == 100);
        }
    }


}
