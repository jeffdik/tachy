module Dndn
  def dndn(sexp)
    if sexp.class == Array
      expanded_sexp = sexp.map { |e| dndn(e) }
      dndn_converter(expanded_sexp)
    else
      sexp
    end
  end
  
  def dndn_converter(sexp)
    if sexp[0].class == Symbol
      case sexp[0].to_s
      when /^([\w\.]+)\.$/
        [:new, [:quote, $1.to_sym]]
      when /^\.(\w+)\$$/
        case sexp.length
        when 2
          [:"get-property", sexp[1], [:quote, $1.to_sym]]
        when 3
          [:"set-property", sexp[1], [:quote, $1.to_sym], sexp[2]]
        end
      when /\w\.\w+$/
        /^(.*)\.([^.]+)$/ =~ sexp[0].to_s
        [:"call-static", [:quote, $1.to_sym], [:quote, $2.to_sym]] + sexp[1..-1]
      else
        sexp
      end
    else
      sexp
    end
  end
end
