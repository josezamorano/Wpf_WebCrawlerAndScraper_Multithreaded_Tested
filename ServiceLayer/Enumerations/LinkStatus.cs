namespace ServiceLayer.Enumerations
{
    public enum LinkStatus
    {
        Found_ForValidation = 1,
        Invalid_External = 2,
        Invalid_Null = 3,
        Invalid_Mailto = 4,
        Invalid_Phone = 5,
        Invalid_Sms = 6,
        Valid_ParentPage = 7,
        Valid_InternalChildPage = 8
    }
}
