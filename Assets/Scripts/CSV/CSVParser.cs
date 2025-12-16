using System.Collections.Generic;
using UnityEngine;

public class CSVParser
{
    public void Load(CSV Target,int startLine = 1)
    {
        // 파일 경로 지정
        //Application.dataPath로 하면 Build했을때 기준 CSV파일을 읽어오지못해서 Resources.Load로 변경
        //string filePath = Application.dataPath + "Resources/Database/CSV/" + Target.Path + ".csv";
        TextAsset csvFile = Resources.Load<TextAsset>("Database/CSV/" + Target.Path);

        // 지정된 경로에 파일이 존재하는 경우에만 읽어야 한다.
        if (csvFile != null)
        //if (File.Exists(filePath))
        {
            // 경로의 파일을 텍스트로 읽어들인다.
            //string csvFile = File.ReadAllText(filePath);

            // 개행문자 단위로 줄을 나눈 뒤, 배열에 담는다.
            string[] lines = csvFile.text.Split('\n');

            // 모든 줄을 순회하면서
            for (int l = startLine; l < lines.Length; l++)
            {
                //비었을경우 넘어간다
                if (lines[l] == "") continue;
                // 한 줄 안에서 쉼표(,)를 기준으로 각 데이터를 나누고 배열에 담는다
                string[] fields = lines[l].Split(',');
                //행마다 마지막 요소에 \r 붙어있어서 Trim으로 지워줌
                if (fields.Length != 0)
                {
                    fields[fields.Length - 1] = fields[fields.Length - 1].Trim();
                }

                // 리스트에 한 줄을 추가하고, 각 셀의 데이터가 들어갈 항목 단위의 리스트를 생성한다.
                List<string> row = new List<string>();
                Target.Data.Add(row);

                // 각 항목 단위로 순회하면서
                for (int f = 0; f < fields.Length; f++)
                {
                    // 항목(셀) 단위의 리스트에 데이터를 추가한다.
                    row.Add(fields[f]);
                }
            }
        }
    }
}

public class CSV
{
    // 파일 경로
    public string Path { get; set; }
    // CSV 파일의 데이터를 2차원 배열의 형태로 보관
    public List<List<string>> Data { get; set; } = new List<List<string>>();

    public CSV(string path)
    {
        Path = path;
    }

    public List<string> GetLine(int Number, ref bool isSuccessLoad)
    {
        // 입력받은 넘버가 CSV데이터의 줄 수보다 많다면
        if (Number > Data.Count - 1)
        {
            return null;
        }

        // 아니라면
        isSuccessLoad = true;
        return Data[Number];  // 2차원 배열의 형태이므로, 한 줄을 통째로 반환한다.
    }
}