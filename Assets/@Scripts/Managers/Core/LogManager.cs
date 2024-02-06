using System;
using System.IO;
using UnityEngine;

public class LogManager : ILogHandler
{
	private FileStream fileStream;
	private StreamWriter streamWriter;
	private ILogHandler defaultLogHandler = Debug.unityLogger.logHandler;

	public void Init()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "GameLogs.log");
		fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
		streamWriter = new StreamWriter(fileStream);

		// 기존 로그 핸들러를 대체
		Debug.unityLogger.logHandler = this;
	}

	public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		// 사용자 정의 로그 메시지 처리
		string logWithTimestamp = DateTime.Now.ToString($"[yyyy-MM-dd HH:mm:ss] {format}");
		streamWriter.WriteLine(string.Format(logWithTimestamp, args));
		streamWriter.Flush();

		// 기본 로그 핸들러를 사용하여 에디터 콘솔에도 로그 출력
		defaultLogHandler.LogFormat(logType, context, format, args);
	}

	public void LogException(System.Exception exception, UnityEngine.Object context)
	{
		// 예외 처리
		streamWriter.WriteLine($"Exception: {exception.Message}");
		streamWriter.Flush();

		// 기본 로그 핸들러를 사용하여 에디터 콘솔에도 예외 로그 출력
		defaultLogHandler.LogException(exception, context);
	}

	// 자원 정리
	public void Close()
	{
		streamWriter.Close();
		fileStream.Close();
	}
}
