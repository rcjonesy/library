using System.ComponentModel.DataAnnotations;

namespace LoncotesLibrary.Models.DTOs;

public class MaterialDTO
{
    public int Id { get; set; }
    [Required]
    public string MaterialName { get; set;}
    [Required]
    public int MaterialTypeId { get; set; }
    public MaterialTypeDTO MaterialType { get; set; }
    
    public int CheckoutId { get; set; }
    public List<CheckoutDTO> Checkouts { get; set; }
    public int GenreId { get; set; }
    public GenreDTO Genre { get; set; }
    public DateTime? OutOfCirculationSince { get; set; }
 }