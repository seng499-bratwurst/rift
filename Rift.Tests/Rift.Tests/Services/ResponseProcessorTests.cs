using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rift.Tests.Services
{
    [TestClass]
    public class ResponseProcessorTests
    {
        private ResponseProcessor _processor = null!;

        [TestInitialize]
        public void Setup()
        {
            _processor = new ResponseProcessor();
        }

        [TestMethod]
        public void ProcessResponse_ExtractsConversationTitle_WhenTitleExists()
        {
            var response = "This is the main response content.\n\n**Conversation Title:** Cambridge Bay Temperature Analysis";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("This is the main response content.", cleanedResponse);
            Assert.AreEqual("Cambridge Bay Temperature Analysis", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_ExtractsConversationTitleWithExtraSpaces_WhenTitleExists()
        {
            var response = "This is the main response content.\n\n**Conversation Title:**    Ice Conditions Marine Life Impact   ";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("This is the main response content.", cleanedResponse);
            Assert.AreEqual("Ice Conditions Marine Life Impact", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_ReturnsSameResponse_WhenNoTitleExists()
        {
            var response = "This is a response without a conversation title.";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("This is a response without a conversation title.", cleanedResponse);
            Assert.IsNull(conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesCaseInsensitive_ConversationTitle()
        {
            var response = "Response content.\n\n**conversation title:** Observatory Environmental Status";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.", cleanedResponse);
            Assert.AreEqual("Observatory Environmental Status", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesEmptyResponse()
        {
            var response = "";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("", cleanedResponse);
            Assert.IsNull(conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_ExtractsConversationTitleAtEndOfResponse()
        {
            var response = "Based on the latest water quality readings from Cambridge Bay Observatory, the conditions appear healthy.\n\n**Conversation Title:** Water Quality Assessment";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Based on the latest water quality readings from Cambridge Bay Observatory, the conditions appear healthy.", cleanedResponse);
            Assert.AreEqual("Water Quality Assessment", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesMultilineResponse_WithConversationTitle()
        {
            var response = @"According to the Oceans 3.0 API response, current ice conditions show 1.2 meters thickness with 85% coverage as of July 17, 2025.

From the Arctic Marine Ecosystems documentation, this level of ice coverage significantly impacts phytoplankton growth and marine food chains by reducing light penetration.

**Conversation Title:** Ice Conditions Marine Life Impact";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            var expectedCleanedResponse = @"According to the Oceans 3.0 API response, current ice conditions show 1.2 meters thickness with 85% coverage as of July 17, 2025.

From the Arctic Marine Ecosystems documentation, this level of ice coverage significantly impacts phytoplankton growth and marine food chains by reducing light penetration.";

            Assert.AreEqual(expectedCleanedResponse, cleanedResponse);
            Assert.AreEqual("Ice Conditions Marine Life Impact", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesOnlyWhitespaceAfterTitle()
        {
            var response = "Response content.\n\n**Conversation Title:** Test Title   \n\n   ";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.", cleanedResponse);
            Assert.AreEqual("Test Title", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesInvalidTitleFormat()
        {
            var response = "Response content.\n\nConversation Title: Invalid Format Without Asterisks";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.\n\nConversation Title: Invalid Format Without Asterisks", cleanedResponse);
            Assert.IsNull(conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesPartialTitleFormat()
        {
            var response = "Response content.\n\n*Conversation Title:** Partial Format";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.\n\n*Conversation Title:** Partial Format", cleanedResponse);
            Assert.IsNull(conversationTitle);
        }

        // Tests to ensure compliance with sys_prompt_large_llm.md requirements
        [TestMethod]
        public void ProcessResponse_ExtractsConversationTitle_WithCorrectFormat()
        {
            var response = "Response content.\n\n**Conversation Title:** Cambridge Bay Temperature Analysis";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.", cleanedResponse);
            Assert.AreEqual("Cambridge Bay Temperature Analysis", conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_ExtractsTitle_WithinWordCountLimits()
        {
            // Test 3 words (minimum)
            var response3 = "Response content.\n\n**Conversation Title:** Temperature Data Analysis";
            var (cleanedResponse3, conversationTitle3) = _processor.ProcessResponse(response3);
            Assert.AreEqual("Temperature Data Analysis", conversationTitle3);

            // Test 9 words (maximum)
            var response9 = "Response content.\n\n**Conversation Title:** Cambridge Bay Observatory Water Quality Environmental Status Monitoring Analysis";
            var (cleanedResponse9, conversationTitle9) = _processor.ProcessResponse(response9);
            Assert.AreEqual("Cambridge Bay Observatory Water Quality Environmental Status Monitoring Analysis", conversationTitle9);
        }

        [TestMethod]
        public void ProcessResponse_HandlesOceanographicParameters_InTitles()
        {
            // Test with temperature parameter
            var tempResponse = "Response content.\n\n**Conversation Title:** Current Cambridge Bay Temperature";
            var (cleanedTempResponse, tempTitle) = _processor.ProcessResponse(tempResponse);
            Assert.AreEqual("Current Cambridge Bay Temperature", tempTitle);

            // Test with ice conditions
            var iceResponse = "Response content.\n\n**Conversation Title:** Cambridge Bay Ice Coverage Analysis";
            var (cleanedIceResponse, iceTitle) = _processor.ProcessResponse(iceResponse);
            Assert.AreEqual("Cambridge Bay Ice Coverage Analysis", iceTitle);

            // Test with water quality
            var qualityResponse = "Response content.\n\n**Conversation Title:** Recent Water Quality Assessment";
            var (cleanedQualityResponse, qualityTitle) = _processor.ProcessResponse(qualityResponse);
            Assert.AreEqual("Recent Water Quality Assessment", qualityTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesLocationContext_InTitles()
        {
            // Test with Cambridge Bay
            var bayResponse = "Response content.\n\n**Conversation Title:** Cambridge Bay Salinity Monitoring";
            var (cleanedBayResponse, bayTitle) = _processor.ProcessResponse(bayResponse);
            Assert.AreEqual("Cambridge Bay Salinity Monitoring", bayTitle);

            // Test with Observatory
            var obsResponse = "Response content.\n\n**Conversation Title:** Observatory Environmental Status Overview";
            var (cleanedObsResponse, obsTitle) = _processor.ProcessResponse(obsResponse);
            Assert.AreEqual("Observatory Environmental Status Overview", obsTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesTimeContext_InTitles()
        {
            // Test with Current
            var currentResponse = "Response content.\n\n**Conversation Title:** Current Temperature Readings";
            var (cleanedCurrentResponse, currentTitle) = _processor.ProcessResponse(currentResponse);
            Assert.AreEqual("Current Temperature Readings", currentTitle);

            // Test with Recent
            var recentResponse = "Response content.\n\n**Conversation Title:** Recent Sensor Data Analysis";
            var (cleanedRecentResponse, recentTitle) = _processor.ProcessResponse(recentResponse);
            Assert.AreEqual("Recent Sensor Data Analysis", recentTitle);

            // Test with Seasonal
            var seasonalResponse = "Response content.\n\n**Conversation Title:** Seasonal Temperature Trends Analysis";
            var (cleanedSeasonalResponse, seasonalTitle) = _processor.ProcessResponse(seasonalResponse);
            Assert.AreEqual("Seasonal Temperature Trends Analysis", seasonalTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesProperTitleCase_Formatting()
        {
            // Test proper title case with articles and prepositions
            var titleCaseResponse = "Response content.\n\n**Conversation Title:** Analysis of Current Water Conditions";
            var (cleanedTitleCaseResponse, titleCaseTitle) = _processor.ProcessResponse(titleCaseResponse);
            Assert.AreEqual("Analysis of Current Water Conditions", titleCaseTitle);

            // Test with coordinating conjunctions
            var conjunctionResponse = "Response content.\n\n**Conversation Title:** Temperature and Salinity Analysis";
            var (cleanedConjunctionResponse, conjunctionTitle) = _processor.ProcessResponse(conjunctionResponse);
            Assert.AreEqual("Temperature and Salinity Analysis", conjunctionTitle);
        }

        [TestMethod]
        public void ProcessResponse_IgnoresInvalidFormat_WithoutDoubleAsterisks()
        {
            var response = "Response content.\n\nConversation Title: Invalid Format Without Asterisks";

            var (cleanedResponse, conversationTitle) = _processor.ProcessResponse(response);

            Assert.AreEqual("Response content.\n\nConversation Title: Invalid Format Without Asterisks", cleanedResponse);
            Assert.IsNull(conversationTitle);
        }

        [TestMethod]
        public void ProcessResponse_HandlesComplexOceanographicTitles()
        {
            // Test complex multi-parameter title
            var complexResponse = "Response content.\n\n**Conversation Title:** Cambridge Bay Water Quality Analysis";
            var (cleanedComplexResponse, complexTitle) = _processor.ProcessResponse(complexResponse);
            Assert.AreEqual("Cambridge Bay Water Quality Analysis", complexTitle);

            // Test sensor troubleshooting title
            var sensorResponse = "Response content.\n\n**Conversation Title:** Observatory Sensor Data Issues";
            var (cleanedSensorResponse, sensorTitle) = _processor.ProcessResponse(sensorResponse);
            Assert.AreEqual("Observatory Sensor Data Issues", sensorTitle);
        }

        [TestMethod]
        public void ProcessResponse_ExtractsTitle_RegardlessOfContent()
        {
            // The processor should extract any title the LLM provides - it's the LLM's responsibility 
            // to follow the sys_prompt_large_llm.md guidelines, not the processor's job to validate
            
            // Test that processor extracts even if title contains forbidden words (LLM shouldn't generate these)
            var forbiddenWordsResponse = "Response content.\n\n**Conversation Title:** Help With Data Request";
            var (cleanedForbiddenResponse, forbiddenTitle) = _processor.ProcessResponse(forbiddenWordsResponse);
            Assert.AreEqual("Help With Data Request", forbiddenTitle);

            // Test that processor extracts even if word count is outside 3-9 range (LLM shouldn't generate these)
            var shortResponse = "Response content.\n\n**Conversation Title:** Data";
            var (cleanedShortResponse, shortTitle) = _processor.ProcessResponse(shortResponse);
            Assert.AreEqual("Data", shortTitle);

            var longResponse = "Response content.\n\n**Conversation Title:** Very Long Title That Exceeds The Nine Word Limit Specified In Guidelines";
            var (cleanedLongResponse, longTitle) = _processor.ProcessResponse(longResponse);
            Assert.AreEqual("Very Long Title That Exceeds The Nine Word Limit Specified In Guidelines", longTitle);
        }

        [TestMethod]
        public void ProcessResponse_ExtractsForbiddenWords_WithoutValidation()
        {
            // According to sys_prompt_large_llm.md, these words are forbidden in titles:
            // Generic Terms: "chat", "conversation", "question", "help", "information", "data request"
            // But the processor should extract them anyway - it's the LLM's responsibility to avoid them

            var chatResponse = "Response content.\n\n**Conversation Title:** Chat Analysis Session";
            var (cleanedChatResponse, chatAnalysisTitle) = _processor.ProcessResponse(chatResponse);
            Assert.AreEqual("Chat Analysis Session", chatAnalysisTitle);

            var conversationResponse = "Response content.\n\n**Conversation Title:** Conversation Data Analysis";
            var (cleanedConversationResponse, conversationTitleExtracted) = _processor.ProcessResponse(conversationResponse);
            Assert.AreEqual("Conversation Data Analysis", conversationTitleExtracted);

            var questionResponse = "Response content.\n\n**Conversation Title:** Question About Temperature";
            var (cleanedQuestionResponse, questionTitle) = _processor.ProcessResponse(questionResponse);
            Assert.AreEqual("Question About Temperature", questionTitle);

            var helpResponse = "Response content.\n\n**Conversation Title:** Help With Data Analysis";
            var (cleanedHelpResponse, helpTitle) = _processor.ProcessResponse(helpResponse);
            Assert.AreEqual("Help With Data Analysis", helpTitle);

            var informationResponse = "Response content.\n\n**Conversation Title:** Information Request Processing";
            var (cleanedInformationResponse, informationTitle) = _processor.ProcessResponse(informationResponse);
            Assert.AreEqual("Information Request Processing", informationTitle);

            var dataRequestResponse = "Response content.\n\n**Conversation Title:** Data Request Analysis";
            var (cleanedDataRequestResponse, dataRequestTitle) = _processor.ProcessResponse(dataRequestResponse);
            Assert.AreEqual("Data Request Analysis", dataRequestTitle);
        }
    }
}
