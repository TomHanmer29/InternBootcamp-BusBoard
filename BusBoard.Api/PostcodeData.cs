﻿namespace BusBoard.Api;

public class PostcodeData
{
    public PostcodeResult result;
}

public class PostcodeResult
{
    public string longitude;
    public string latitude;
}

public class PostcodeValidation
{
    public bool result;
}