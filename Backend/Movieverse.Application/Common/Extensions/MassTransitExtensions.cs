﻿using MassTransit;

namespace Movieverse.Application.Common.Extensions;

public class EntityNameFormatter : IEntityNameFormatter
{
	private readonly string _formatter;
	
	public EntityNameFormatter(string formatter)
	{
		_formatter = formatter;
	}

	public string FormatEntityName<T>() => _formatter;
}