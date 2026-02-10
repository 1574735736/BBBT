
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;
using System.Globalization;


	public static class CsvLoader
	{
        public static string ReadDataByResources(string _path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(_path);
            string content = string.Empty;
            if (textAsset)
            {
                content = Encoding.UTF8.GetString(textAsset.bytes);
            }
            else
            {
                Debug.LogError("not found json file");
            }

            if (content.Equals(string.Empty))
            {
                Debug.LogWarning("file not exit or empty");
            }
            content = content.TrimStart('\uFEFF');
            return content;
        }

        public static List<T> ParseCsvConfig<T>(string[] csvContent, char delimiter = '\t') where T : new()
        {
            var result = new List<T>();
            var properties = typeof(T).GetProperties();

            for (int i = 3; i < csvContent.Length; i++) // 从第四行开始解析
            {
                var values = csvContent[i].Split(delimiter); // 根据你的数据格式分割，可能是逗号或其他字符

                var obj = new T();
                for (int j = 0; j < properties.Length; j++)
                {
                    if (j < values.Length)
                    {
                        var property = properties[j];
                        var value = values[j];
                        try
                        {
                            if (property.PropertyType == typeof(int))
                            {
                                property.SetValue(obj, int.Parse(value, CultureInfo.InvariantCulture));
                            }
                            else if (property.PropertyType == typeof(float))
                            {
                                property.SetValue(obj, float.Parse(value, CultureInfo.InvariantCulture));
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                property.SetValue(obj, bool.Parse(value));
                            }
                            else if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(obj, value);
                            }
                            else if (property.PropertyType == typeof(int[]))
                            {
                                var intArray = value.Split('|').Select(int.Parse).ToArray();
                                property.SetValue(obj, intArray);
                            }
                            else if (property.PropertyType == typeof(List<int>))
                            {
                                var intList = value.Split('|').Select(int.Parse).ToList();
                                property.SetValue(obj, intList);
                            }
                            else if (property.PropertyType == typeof(List<float>))
                            {
                                var intList = value.Split('|').Select(float.Parse).ToList();
                                property.SetValue(obj, intList);
                            }
                            else if (property.PropertyType == typeof(List<string>))
                            {
                                var stringList = value.Split('|').Select(v => v.Trim()).ToList();
                                property.SetValue(obj, stringList);
                            }
                            else if (property.PropertyType == typeof(List<GetRmbItem>))
                            {
                                var rmbItems = value.Split('|')
                                    .Select(rmbItem => rmbItem.Split('#'))
                                    .Select(rmbPair => new GetRmbItem
                                    {
                                        Cost = int.Parse(rmbPair[0]),
                                        Amount = int.Parse(rmbPair[1])
                                    })
                                    .ToList();
                                property.SetValue(obj, rmbItems);
                            }
                            else if (property.PropertyType == typeof(List<float>))
                            {
                                var floatList = value.Split('|').Select(float.Parse).ToList();
                                property.SetValue(obj, floatList);
                            }
                        // 可以根据需要添加更多类型转换
                    }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to set property {property.Name} with value {value}: {ex.Message}");
                        }
                    }
                }
                result.Add(obj);
            }
            return result;
        }

        public static List<T> ParseCsvConfigFromContent<T>(string csvContent, char fieldDelimiter = '\t') where T : new()
        {
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return ParseCsvConfig<T>(lines, fieldDelimiter);
        }
    }

   

