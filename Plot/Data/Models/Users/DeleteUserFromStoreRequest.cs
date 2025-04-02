/*
    Filename: DeleteUserFromStoreRequest.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Users

    File Purpose:
    This file contains the object to hold the Access attributes.
    
    Class Purpose:
    This record is used as the file 
    to pass back the values to remove
    a user from a store.
    
    Written by: Josh Rodack
*/



namespace Plot.Data.Models.Users;

public record DeleteUserFromStoreRequest
{
    public int userid { get; set; }
    public int storeid { get; set; }
}