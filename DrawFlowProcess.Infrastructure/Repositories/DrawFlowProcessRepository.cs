using DrawFlowProcess.Domain.Domain;
using DrawFlowProcess.Domain.Interface;
using DrawFlowProcess.Domain.Repositories;
using DrawFlowProcess.Infrastructure.Utilities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using System.Text.Json.Nodes;
using UAParser;

namespace DrawFlowProcess.Infrastructure.Repositories
{
    public class DrawFlowProcessRepository : IDrwaFlowProcessRepository
    {

        private readonly IMongoCollection<BsonDocument> _collection;
        private readonly IClientInfoRepository _clientInfoRepository;

        public DrawFlowProcessRepository(IMongoContext context, IClientInfoRepository clientInfoRepository)
        {
            _collection = context.GetDatabase().GetCollection<BsonDocument>("WorkFlow");
            _clientInfoRepository = clientInfoRepository;
        }

        public async Task<ExportJson> GetJsonByIds(Guid agreementId, int workFlowId)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D").ToLower()),
                Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId));

            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            if (document != null)
            {
                var documentString = document.ToJson();

                var result = GetJsonConvert(JsonDocument.Parse(documentString));

                return result;
            }

            return null!;
        }

        public async Task<bool> SaveJsonConvert(ExportJson exportJson)
        {
            try
            {
                var json = JsonSerializer.Serialize(exportJson);
                var bson = BsonDocument.Parse(json);
                await _collection.InsertOneAsync(bson);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ExportJson GetJsonConvert(JsonDocument jsonDocument)
        {
            ExportJson exportJson = new();

            var jsonString = jsonDocument.RootElement.GetRawText();
            var bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonString);


            var data = bson.GetValue("drawflow", null)?.AsBsonDocument
               .GetValue("drawflow", null)?.AsBsonDocument
               .GetValue("Home", null)?.AsBsonDocument
               .GetValue("data", null)?.AsBsonDocument
           ?? null;

            if (data == null)
                data = bson["drawflow"]["Home"]["data"].AsBsonDocument;

            exportJson.AgreementID = new Guid(bson["AgreementID"].AsString);
            exportJson.WorkflowID = bson["WorkFlowID"].AsInt32;

            List<Nodo> nodeList = [];

            foreach (var element in data)
            {
                var val = element.Value.AsBsonDocument;

                Nodo node = new()
                {
                    Id = val["id"].AsInt32,
                    Type = val["name"].AsString,
                    Connections = GetConnection(val, val["id"].AsInt32),
                    Data = BsonTypeMapper.MapToDotNetValue(val["data"].AsBsonDocument) as Dictionary<string, object>
                };

                nodeList.Add(node);
            }

            exportJson.Nodos = OrderNodes(nodeList);

            return exportJson;
        }

        public async Task<ProcessFlow> GetProcessFlow(Guid agreementId, int workFlowId, string typeCurrent, JsonDocument conditional = null!, int typeProcess = 0, bool back = false)
        {
            ExportJson? model = await GetJsonByIds(agreementId, workFlowId);
            var result = FilterProcessFlow(model, typeCurrent, conditional, typeProcess, back);
            return result;
        }

        public async Task<JsonPages> GetJsonPages(Guid agreementId, int workFlowId)
        {
            var json = await GetJsonByIds(agreementId, workFlowId);

            var pages = json.Nodos!.Select(s => s.Data).ToList();

            var jsonpP = new JsonPages();

            var pagesResult = ValidateDataBool(pages);

            Dictionary<string, object> pairs = new()
            {
                { "pages", pagesResult }
            };

            jsonpP.CountPages = pagesResult.Count;
            jsonpP.Pages = pairs;

            return jsonpP;
        }

        // Inicio refactorización/optimización por GitHub Copilot
        // Método generado por GitHub Copilot
        private static JsonObject ValidateDataBool(List<Dictionary<string, object>> pairs)
        {
            var result = new JsonObject();
            var keysAdded = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (pairs == null || pairs.Count == 0)
                return result;

            foreach (var pages in GetValidPages(pairs))
            {
                AddPageValues(result, keysAdded, pages);
            }

            result.Add("generateqr", true);
            return result;
        }

        // Método generado por GitHub Copilot
        private static IEnumerable<Dictionary<string, object>> GetValidPages(
            IEnumerable<Dictionary<string, object>> pairs
        )
        {
            foreach (var dic in pairs)
            {
                if (dic == null)
                    continue;

                if (!dic.TryGetValue("pages", out var pagesObj))
                    continue;

                if (pagesObj is Dictionary<string, object> pages && pages.Count > 0)
                    yield return pages;
            }
        }

        private static void AddPageValues(
            JsonObject result,
            HashSet<string> keysAdded,
            Dictionary<string, object> pages
        )
        {
            foreach (var page in pages)
            {
                if (keysAdded.Contains(page.Key))
                    continue;

                if (TryAddBooleanValue(result, keysAdded, page))
                    continue;

                TryAddResultValidation(result, keysAdded, page);
            }
        }

        private static bool TryAddBooleanValue(
            JsonObject result,
            HashSet<string> keysAdded,
            KeyValuePair<string, object> page
        )
        {
            if (page.Value is not bool boolValue)
                return false;

            result.Add(page.Key, boolValue);
            keysAdded.Add(page.Key);
            return true;
        }

        private static void TryAddResultValidation(
            JsonObject result,
            HashSet<string> keysAdded,
            KeyValuePair<string, object> page
        )
        {
            if (!page.Key.Contains("resultvalidation", StringComparison.OrdinalIgnoreCase))
                return;

            result.Add(page.Key, true);
            keysAdded.Add(page.Key);
        }
        // Fin refactorización/optimización por GitHub Copilot

        private static Connection GetConnection(BsonDocument connections, int nodeStart)
        {
            Connection connection = new();

            List<Point> inputs = [];
            List<Point> outputs = [];

            foreach (var element in connections["inputs"].AsBsonDocument)
            {
                var val = element.Value.AsBsonDocument;

                foreach (var item in val["connections"].AsBsonArray)
                {
                    var itemval = item.AsBsonDocument;

                    Point point = new()
                    {
                        NodeIdStart = nodeStart,
                        NodeIdEnd = int.Parse(itemval["node"].AsString)
                    };

                    inputs.Add(point);
                }
            }

            connection.Inputs = inputs;

            foreach (var element in connections["outputs"].AsBsonDocument)
            {
                var val = element.Value.AsBsonDocument;

                foreach (var item in val["connections"].AsBsonArray)
                {
                    var itemval = item.AsBsonDocument;

                    Point point = new()
                    {
                        NodeIdStart = nodeStart,
                        NodeIdEnd = int.Parse(itemval["node"].AsString)
                    };

                    outputs.Add(point);
                }
            }

            connection.Outputs = outputs;

            return connection;
        }

        // Inicio refactorización/optimización por GitHub Copilot
        // Método generado por GitHub Copilot
        private static List<Nodo> OrderNodes(List<Nodo> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return [];

            var nodeById = nodes.ToDictionary(n => n.Id);

            var nodeStart = nodes.FirstOrDefault(n =>
                n.Type.Equals("Inicio", StringComparison.OrdinalIgnoreCase) ||
                (n.Connections?.Inputs == null || n.Connections.Inputs.Count == 0));

            if (nodeStart == null)
                return [];

            return TraverseFrom(nodeStart, nodeById);
        }

        private static List<Nodo> TraverseFrom(Nodo start, Dictionary<int, Nodo> nodeById)
        {
            var orderedNodes = new List<Nodo>();
            var visited = new HashSet<int>();
            var stack = new Stack<Nodo>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (!visited.Add(current.Id))
                    continue;

                orderedNodes.Add(current);

                var outputs = current.Connections?.Outputs;
                if (outputs == null || outputs.Count == 0)
                    continue;

                for (int i = outputs.Count - 1; i >= 0; i--)
                {
                    var conn = outputs[i];
                    if (nodeById.TryGetValue(conn.NodeIdEnd, out var child) && !visited.Contains(child.Id))
                    {
                        stack.Push(child);
                    }
                }
            }

            return orderedNodes;
        }
        // Fin refactorización/optimización por GitHub Copilot

        // Inicio refactorización/optimización por GitHub Copilot
        // Método generado por GitHub Copilot
        private ProcessFlow FilterProcessFlow(ExportJson json, string typeCurrent, JsonDocument valueReturn = null!, int typeProcess = 0, bool back = false)
        {
            if (json == null || json.Nodos == null || json.Nodos.Count == 0)
                return new ProcessFlow();

            (ProcessFlow result, Nodo? currentFlow) = ResolveResultAndCurrentNode(json, typeCurrent, valueReturn, typeProcess, back);

            var pagesC = GetPagesFromSelectedPath(json, typeProcess);
            int countPages = CountTruePages(pagesC);

            var (pagesCurrent, atdp, initVal, signature, confingRefuzed) = ExtractCurrentFlowData(currentFlow);

            JsonObject resultInitVal = [];
            if (initVal != null && initVal.Count != 0)
            {
                resultInitVal = ValidationInitReuslt(initVal);

                bool anyDeviceTrue = resultInitVal["Divice"]?
                    .AsObject()
                    .Where(kv => kv.Value is JsonValue val && val.TryGetValue(out bool b) && b)
                    .Any() ?? false;

                if (!anyDeviceTrue)
                {
                    pagesCurrent.Clear();
                    pagesCurrent.Add("generateqr", true);
                    countPages = 1;
                }
            }

            var resultData = new JsonObject()
            {
                ["pages"] = pagesCurrent
            };

            if (atdp.Count > 0) resultData["atdp"] = atdp;
            if (initVal?.Count > 0) resultData["initValidation"] = resultInitVal;
            if (signature?.Count > 0) resultData["signature"] = signature;
            if (confingRefuzed?.Count > 0) resultData["config"] = confingRefuzed;

            result.DataConfiguration = resultData;
            result.CountPages = countPages;

            return result;
        }

        private (ProcessFlow result, Nodo? currentFlow) ResolveResultAndCurrentNode(ExportJson json, string typeCurrent, JsonDocument valueReturn, int typeProcess, bool back)
        {
            var result = NodePear(json, typeCurrent);
            Nodo? currentFlow = json.Nodos!.FirstOrDefault(f => f.Type.ToLower() == result.TypeFrom);

            if (back && !typeCurrent.Contains("inicio"))
            {
                if (currentFlow != null && currentFlow.Type.Contains(Constants.Conditional))
                {
                    var backType = result.TypeBack?.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(backType))
                    {
                        currentFlow = json.Nodos!.FirstOrDefault(f => f.Type.ToLower() == backType);
                        if (currentFlow != null)
                            result = NodePear(json, currentFlow.Type);
                    }
                }
            }
            else if (typeCurrent.Contains(Constants.Conditional))
            {
                var stepBack = result.TypeBack;
                result = CondicionalFlow(json, currentFlow!, result, valueReturn, typeProcess);
                result.TypeBack = stepBack;
                currentFlow = json.Nodos!.FirstOrDefault(f => f.Type.ToLower() == result.TypeFrom);
            }

            return (result, currentFlow);
        }

        private static int CountTruePages(IEnumerable<Dictionary<string, object>> pagesCollection)
        {
            int count = 0;
            foreach (var page in pagesCollection)
            {
                foreach (var kv in page)
                {
                    if (kv.Value is bool b && b)
                        count++;
                }
            }
            return count;
        }

        // Método generado por GitHub Copilot
        private static (JsonObject pagesCurrent, JsonObject atdp, JsonObject initVal, JsonObject signature, JsonObject confingRefuzed) ExtractCurrentFlowData(
            Nodo? currentFlow
        )
        {
            var pagesCurrent = new JsonObject();
            var atdp = new JsonObject();
            var initVal = new JsonObject();
            var signature = new JsonObject();
            var confingRefuzed = new JsonObject();

            if (currentFlow?.Data == null)
                return (pagesCurrent, atdp, initVal, signature, confingRefuzed);

            var targetMap = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase)
            {
                ["pages"] = pagesCurrent,
                ["atdp"] = atdp,
                ["initval"] = initVal,
                ["docConfig"] = signature,
                ["configuration"] = confingRefuzed
            };

            foreach (var pair in currentFlow.Data)
            {
                TryAddDictionaryValues(pair, targetMap);
            }

            return (pagesCurrent, atdp, initVal, signature, confingRefuzed);
        }

        // Método generado por GitHub Copilot
        private static void TryAddDictionaryValues(
            KeyValuePair<string, object> pair,
            IReadOnlyDictionary<string, JsonObject> targetMap
        )
        {
            if (!targetMap.TryGetValue(pair.Key, out var target))
                return;

            if (pair.Value is not Dictionary<string, object> values)
                return;

            foreach (var item in values)
            {
                target.Add(item.Key, JsonValue.Create(item.Value));
            }
        }
        // Fin refactorización/optimización por GitHub Copilot

        // Inicio refactorización/optimización por GitHub Copilot
        // Método generado por GitHub Copilot
        private ProcessFlow CondicionalFlow(ExportJson json, Nodo currentFlow, ProcessFlow result, JsonDocument valueReturn = null!, int typeProcess = 0)
        {
            if (currentFlow?.Data == null || result?.TypeBack == null || result.TypeBack.Count == 0)
            {
                var fallback = result?.TypeFront?.ElementAtOrDefault(1) ?? result?.TypeFront?.FirstOrDefault() ?? result!.TypeFrom;
                return NodePear(json, fallback!);
            }

            if (!TryGetConditionalDictionary(currentFlow.Data, out var conditionalDict))
            {
                var fallback = result?.TypeFront?.ElementAtOrDefault(1) ?? result?.TypeFront?.FirstOrDefault() ?? result!.TypeFrom;
                return NodePear(json, fallback!);
            }

            var subValidationKey = result.TypeBack![0].Split('-')[0].ToLowerInvariant();
            var subPair = conditionalDict!.TryGetValue(subValidationKey, out var spObj) ? spObj as Dictionary<string, object> : null;
            bool validationResult = EvaluateSubValidation(subValidationKey, subPair, valueReturn, typeProcess);

            var trueBranch = result.TypeFront?.ElementAtOrDefault(0) ?? result.TypeFrom;
            var falseBranch = result.TypeFront?.ElementAtOrDefault(1) ?? trueBranch;

            return validationResult ? NodePear(json, trueBranch!) : NodePear(json, falseBranch!);
        }

        // Método generado por GitHub Copilot
        private static bool TryGetConditionalDictionary(Dictionary<string, object> nodeData, out Dictionary<string, object>? conditionalDict)
        {
            conditionalDict = null;
            if (!nodeData.TryGetValue(Constants.Conditional, out var condObj))
                return false;

            conditionalDict = condObj as Dictionary<string, object>;
            return conditionalDict != null;
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateSubValidation(string subValidationKey, Dictionary<string, object>? subPair, JsonDocument valueReturn, int typeProcess)
        {
            if (subPair == null || valueReturn == null)
                return false;

            JsonElement root = valueReturn.RootElement;

            return subValidationKey switch
            {
                var s when s.Equals(Constants.SubValidationFacial, StringComparison.OrdinalIgnoreCase) =>
                    EvaluateFacialValidation(subPair, root, typeProcess),

                var s when s.Equals(Constants.SubValidationDocuement, StringComparison.OrdinalIgnoreCase) =>
                    EvaluateDocumentValidation(subPair, root),

                var s when s.Equals(Constants.SubInicio, StringComparison.OrdinalIgnoreCase) =>
                    EvaluateSubInicio(subPair, typeProcess),

                _ => false
            };
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateFacialValidation(
            Dictionary<string, object> subPair,
            JsonElement root,
            int typeProcess
        )
        {
            return typeProcess switch
            {
                Constants.Enrrolment => EvaluateEnrollment(subPair, root),
                Constants.Validation => EvaluateValidation(subPair, root),
                _ => false
            };
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateEnrollment(
            Dictionary<string, object> subPair,
            JsonElement root
        )
        {
            if (!TryGetBooleanValue(root, out var valueRF))
                return false;

            var expectedStr = subPair.TryGetValue(Constants.ResultCalculate, out var rc)
                ? rc?.ToString()
                : null;

            var expected = string.Equals(expectedStr, "true", StringComparison.OrdinalIgnoreCase);
            return expected == valueRF;
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateValidation(
            Dictionary<string, object> subPair,
            JsonElement root
        )
        {
            if (!TryGetSelectAndValue(subPair, out var selectCalculate, out var valueNumeric))
                return false;

            if (!TryGetRootValue(root, out var rootValue))
                return false;

            return CalculateCondition(
                selectCalculate,
                valueNumeric,
                JsonDocument.Parse($"{{\"value\":\"{rootValue}\"}}"));
        }

        // Método generado por GitHub Copilot
        private static bool TryGetBooleanValue(JsonElement root, out bool value)
        {
            value = default;

            if (!root.TryGetProperty("value", out var valElement))
                return false;

            if (valElement.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
                return false;

            value = valElement.GetBoolean();
            return true;
        }

        // Método generado por GitHub Copilot
        private static bool TryGetSelectAndValue(
            Dictionary<string, object> subPair,
            out string selectCalculate,
            out int valueNumeric
        )
        {
            selectCalculate = subPair.TryGetValue(Constants.SelectCalculate, out var sc)
                ? sc?.ToString()!
                : null!;

            valueNumeric = subPair.TryGetValue(Constants.ValueNumeric, out var vn)
                ? Convert.ToInt32(vn)
                : 0;

            return !string.IsNullOrWhiteSpace(selectCalculate);
        }

        // Método generado por GitHub Copilot
        private static bool TryGetRootValue(JsonElement root, out string value)
        {
            value = null!;

            if (!root.TryGetProperty("value", out var rootVal))
                return false;

            value = rootVal.GetString()!;
            return value != null;
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateDocumentValidation(Dictionary<string, object> subPair, JsonElement root)
        {
            if (!root.TryGetProperty("value", out var valElement) || (valElement.ValueKind != JsonValueKind.True && valElement.ValueKind != JsonValueKind.False))
                return false;

            var valueR = valElement.GetBoolean();
            var expected = subPair.TryGetValue(Constants.ResponseValue, out var rv) ? rv?.ToString() : null;
            bool expectedBool = string.Equals(expected, "true", StringComparison.OrdinalIgnoreCase);
            return expectedBool == valueR;
        }

        // Método generado por GitHub Copilot
        private static bool EvaluateSubInicio(Dictionary<string, object> subPair, int typeProcess)
        {
            var firstRouteObj = subPair.TryGetValue(Constants.FirstRoute, out var fr) ? fr : null;
            if (firstRouteObj == null)
                return false;

            var firstRoute = Convert.ToInt32(firstRouteObj);
            if (typeProcess == Constants.Validation)
                return firstRoute == Constants.Validation;

            if (typeProcess == Constants.Enrrolment)
                return firstRoute == Constants.Enrrolment;

            return false;
        }
        // Fin refactorización/optimización por GitHub Copilot

        // Inicio refactorización/optimización por GitHub Copilot
        // Método generado por GitHub Copilot
        private static List<Dictionary<string, object>> GetPagesFromSelectedPath(
            ExportJson json,
            int typeProcess
        )
        {
            var pagesList = new List<Dictionary<string, object>>();

            if (!IsValidJson(json))
                return pagesList;

            var nodeMap = json.Nodos!.ToDictionary(n => n.Id.ToString());
            var startNode = GetStartNode(json.Nodos!);

            if (startNode == null)
                return pagesList;

            TraverseGraph(startNode.Id.ToString(), nodeMap, pagesList, typeProcess);

            return pagesList;
        }

        // Método generado por GitHub Copilot
        private static Nodo? GetStartNode(IEnumerable<Nodo> nodes)
        {
            return nodes.FirstOrDefault(n =>
                n.Type.Contains("inicio", StringComparison.OrdinalIgnoreCase));
        }

        // Método generado por GitHub Copilot
        private static bool IsValidJson(ExportJson json)
        {
            return json?.Nodos != null && json.Nodos.Count > 0;
        }

        // Método generado por GitHub Copilot
        private static void TraverseGraph(
            string startNodeId,
            Dictionary<string, Nodo> nodeMap,
            List<Dictionary<string, object>> pagesList,
            int typeProcess
        )
        {
            var visited = new HashSet<string>();
            var stack = new Stack<string>();
            stack.Push(startNodeId);

            while (stack.Count > 0)
            {
                var nodeId = stack.Pop();

                if (!TryGetNode(nodeId, nodeMap, visited, out var node))
                    continue;

                AddPagesIfExist(node, pagesList);
                PushNextNodes(node, nodeMap, stack, typeProcess);
            }
        }

        // Método generado por GitHub Copilot
        private static bool TryGetNode(
            string nodeId,
            Dictionary<string, Nodo> nodeMap,
            HashSet<string> visited,
            out Nodo node
        )
        {
            node = null!;

            if (!nodeMap.TryGetValue(nodeId, out var found) || visited.Contains(nodeId))
                return false;

            visited.Add(nodeId);
            node = found;
            return true;
        }

        // Método generado por GitHub Copilot
        private static void AddPagesIfExist(
            Nodo node,
            List<Dictionary<string, object>> pagesList
        )
        {
            if (node.Data == null ||
                !node.Data.TryGetValue("pages", out var pageObj) ||
                pageObj is not Dictionary<string, object> pageDict)
                return;

            var filtered = pageDict
                .Where(kvp => !kvp.Key.Equals("captureid", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (filtered.Count > 0)
                pagesList.Add(filtered);
        }

        // Método generado por GitHub Copilot
        private static void PushNextNodes(
            Nodo node,
            Dictionary<string, Nodo> nodeMap,
            Stack<string> stack,
            int typeProcess
        )
        {
            var outputs = node.Connections?.Outputs;
            if (outputs == null || outputs.Count == 0)
                return;

            bool isConditional = node.Type.Contains("condicion-1", StringComparison.OrdinalIgnoreCase);
            int? allowedOutput = isConditional
                ? GetConditionalOutput(node, typeProcess)
                : null;

            for (int i = outputs.Count - 1; i >= 0; i--)
            {
                var targetId = outputs[i].NodeIdEnd.ToString();

                if (allowedOutput.HasValue && outputs[i].NodeIdEnd != allowedOutput.Value)
                    continue;

                if (nodeMap.ContainsKey(targetId))
                    stack.Push(targetId);
            }
        }

        // Método generado por GitHub Copilot
        private static int GetConditionalOutput(Nodo node, int typeProcess)
        {
            return typeProcess == 1
                ? node.Connections.Outputs![0].NodeIdEnd
                : node.Connections.Outputs![1].NodeIdEnd;
        }
        // Fin refactorización/optimización por GitHub Copilot

        private JsonObject ValidationInitReuslt(JsonObject value)
        {
            using var doc = JsonDocument.Parse(value.ToJsonString());
            var root = doc.RootElement;

            var getNav = _clientInfoRepository.GetBrowser();
            var getOs = _clientInfoRepository.GetOS();
            var getDiv = _clientInfoRepository.GetDevice();

            var transformed = new JsonObject();

            var navObj = new JsonObject();
            if (root.TryGetProperty("miSelectMultipleNav", out var navArray) && navArray.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in navArray.EnumerateArray())
                {
                    var nav = item.GetString() ?? "";
                    navObj[nav] = string.Equals(nav, getNav, StringComparison.OrdinalIgnoreCase);
                }
            }

            var deviceObj = new JsonObject();
            if (root.TryGetProperty("miSelectMultiple", out var deviceArray) && deviceArray.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in deviceArray.EnumerateArray())
                {
                    var dev = item.GetString() ?? "";
                    bool match = getDiv.Contains(dev, StringComparison.OrdinalIgnoreCase)
                              || getOs.Contains(dev, StringComparison.OrdinalIgnoreCase);
                    deviceObj[dev] = match;
                }
            }

            transformed["Navigate"] = navObj;
            transformed["Divice"] = deviceObj;
            transformed["action"] = root.TryGetProperty("action", out var act) ? act.GetString() : "";

            return transformed;
        }

        private static bool CalculateCondition(string prepo, int valueExpected, JsonDocument returnResult)
        {
            if (returnResult == null)
                return false;

            JsonElement root = returnResult.RootElement;

            if (!root.TryGetProperty("value", out var valueElement))
                return false;

            string? raw = valueElement.ValueKind switch
            {
                JsonValueKind.String => valueElement.GetString(),
                JsonValueKind.Number => valueElement.GetRawText(),
                JsonValueKind.True => "1",
                JsonValueKind.False => "0",
                _ => valueElement.GetRawText()
            };

            if (!int.TryParse(raw, out var actual))
                return false;

            return prepo switch
            {
                ">" => actual > valueExpected,
                "<" => actual < valueExpected,
                "=" or "==" => actual == valueExpected,
                "!=" => actual != valueExpected,
                _ => false
            };
        }

        private static ProcessFlow NodePear(ExportJson json, string typeCurrent)
        {
            var result = new ProcessFlow();

            if (json == null || json.Nodos == null || string.IsNullOrWhiteSpace(typeCurrent))
                return result;

            var currentFlow = json.Nodos.FirstOrDefault(f => string.Equals(f.Type, typeCurrent, StringComparison.OrdinalIgnoreCase));
            if (currentFlow == null)
                return result;

            var currentOuts = currentFlow.Connections?.Outputs?.Select(s => s.NodeIdEnd).ToHashSet();
            var currentInts = currentFlow.Connections?.Inputs?.Select(s => s.NodeIdEnd).ToHashSet();

            if (currentOuts != null && currentOuts.Count > 0)
            {
                var nextFlow = json.Nodos.Where(w => currentOuts.Contains(w.Id)).ToList();
                result.TypeFront = nextFlow.Select(s => s.Type).ToList();

                if (result.TypeFront.Count > 0 && result.TypeFront[0].Split('-')[0].Contains(Constants.Conditional))
                {
                    result.Conditional = true;
                }
            }

            if (currentInts != null && currentInts.Count > 0)
            {
                var backFlow = json.Nodos.Where(w => currentInts.Contains(w.Id)).ToList();
                result.TypeBack = backFlow.Select(s => s.Type).ToList();
            }

            result.TypeFrom = currentFlow.Type;
            return result;
        }
    }
}
