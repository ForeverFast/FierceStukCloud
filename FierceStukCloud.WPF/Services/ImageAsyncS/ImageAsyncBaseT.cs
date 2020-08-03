using System.Windows.Threading;

namespace FierceStukCloud.Wpf.Services.ImageAsyncS
{
	/// <summary>Базовый класс с ImageSource загружаемым
	/// ассинхронно после первого требования и возможностью добавления дополнительного Контента</summary>
	/// <typeparam name="T">Тип Контента</typeparam>
	public abstract class ImageAsyncBase<T> : ImageAsyncBase
	{
		private T _content;

		protected ImageAsyncBase(Dispatcher dispatcher)
			: base(dispatcher) { }

		public T Content { get => _content; set => SetProperty(ref _content, value); }
	}
}
